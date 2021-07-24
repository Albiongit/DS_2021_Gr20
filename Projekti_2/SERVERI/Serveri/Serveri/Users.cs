using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Serveri
{
    class Users
    {
        public String Emri { get; set; }
        public String Mbiemri { get; set; }
        public String Username { get; set; }
        public String SaltedHash { get; set; }
        public String InvoiceType { get; set; }
        public String Year { get; set; }
        public String email { get; set; }
        public String Month { get; set; }
        public String VleraEuro { get; set; }
        public String Shteti { get; set; }

       public String Salt { get; set; }
        
        public  Users(String Emri,String Mbiemri,String Username,string SaltedHash,string InvoiceType,string Year,
            string email,string Month,string VleraEuro,string Shteti,string Salt)
        {
            this.Emri = Emri;
            this.Mbiemri = Mbiemri;
            this.Username = Username;
            this.SaltedHash = SaltedHash;
            this.InvoiceType = InvoiceType;
            this.Year = Year;
            this.email = email;
            this.Month = Month;
            this.VleraEuro = VleraEuro;
            this.Shteti = Shteti;
            this.Salt = Salt;
            
        }
        public static string generateSalt()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            return rnd.Next(100000, 1000000).ToString();
        }

        public static string generateSaltedHash(string password, string salt)
        {
            string passwordSalt = password + salt;
            SHA1CryptoServiceProvider objHash = new SHA1CryptoServiceProvider();

            byte[] bytePasswordSalt = Encoding.UTF8.GetBytes(passwordSalt);
            byte[] byteSaltedPasswordHash = objHash.ComputeHash(bytePasswordSalt);

            return Convert.ToBase64String(byteSaltedPasswordHash);
        }

        public static List<Users> KontrolloUsers()
        {
            string filename = @"C:\Users\GJKK\Desktop\PROJETI DS\SERVERI\Serveri\Serveri\bin\Debug\netcoreapp3.1\Users.json";
            if (File.Exists(filename))
            {
                var users = JsonConvert.DeserializeObject<List<Users>>(File.ReadAllText(filename));
                return users;
            }
            return null;
        }
        public static void insert(String Emri, String Mbiemri, String Username,string Password, string SaltedHash, string InvoiceType, string Year,
            string email, string Month, string VleraEuro, string Shteti, string Salt)
        {
            if (File.Exists("users.xml") == false)
            {
                XmlTextWriter xmlTw = new XmlTextWriter("users.xml", Encoding.UTF8);
                xmlTw.WriteStartElement("users");
                xmlTw.Close();
            }
            XmlDocument objXml = new XmlDocument();
            objXml.Load("users.xml");

            XmlElement rootNode = objXml.DocumentElement;
            XmlElement userNode = objXml.CreateElement("useri");
            XmlElement emriNode = objXml.CreateElement("emri");
            XmlElement mbiemriNode = objXml.CreateElement("mbiemri");
            XmlElement usernameNode = objXml.CreateElement("username");
            XmlElement passwordNode = objXml.CreateElement("passowrd");
            XmlElement saltedHashNode = objXml.CreateElement("saltedHash");
            XmlElement invoiceTypeNode = objXml.CreateElement("invoiceType");
            XmlElement yearNode = objXml.CreateElement("year");
            XmlElement emailNode = objXml.CreateElement("email");
            XmlElement monthNode = objXml.CreateElement("month");
            XmlElement vleraEuroNode = objXml.CreateElement("vleraEuro");
            XmlElement shtetiNode = objXml.CreateElement("shteti");
            XmlElement saltNode = objXml.CreateElement("salt");


            emriNode.InnerText = Emri;
            mbiemriNode.InnerText = Mbiemri;
            usernameNode.InnerText = Username;
            saltedHashNode.InnerText = generateSaltedHash(Password,Salt);
            invoiceTypeNode.InnerText = InvoiceType;
            yearNode.InnerText = Year;
            emailNode.InnerText = email;
            monthNode.InnerText = Month;
            vleraEuroNode.InnerText = VleraEuro;
            shtetiNode.InnerText = Shteti;
            saltNode.InnerText = Salt; 

            passwordNode.AppendChild(saltNode);
            passwordNode.AppendChild(saltedHashNode);
            userNode.AppendChild(emriNode);
            userNode.AppendChild(mbiemriNode);
            userNode.AppendChild(usernameNode);
            userNode.AppendChild(passwordNode);
            userNode.AppendChild(invoiceTypeNode);
            userNode.AppendChild(yearNode);
            userNode.AppendChild(emailNode);
            userNode.AppendChild(monthNode);
            userNode.AppendChild(vleraEuroNode);
            userNode.AppendChild(shtetiNode);

            

            rootNode.AppendChild(userNode);

            objXml.Save("users.xml");

        }
        public static XElement getUserInfo(string username)
        {
            var filename = "users.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var usersFilepath = Path.Combine(currentDirectory, filename);

            try
            {
                XElement users = XElement.Load($"{usersFilepath}");
                IEnumerable<XElement> infoUserat = from useri in users.Descendants("useri")
                                                     where useri.Element("username").Value.ToString().Equals(username)
                                                     select useri;

                return infoUserat.First();
            }
            catch
            {
                return null;
            }
        }
    }
}
