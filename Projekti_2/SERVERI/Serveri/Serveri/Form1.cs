using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serveri
{
    public partial class Form1 : Form
    {
        X509Certificate2 certifikata;
        RSACryptoServiceProvider objRsa = new RSACryptoServiceProvider();
        DESCryptoServiceProvider objDes = new DESCryptoServiceProvider();
        string key;
        string iv;
        Socket serverSocket;
        Socket accept;


        Socket socket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            serverSocket = socket();
           
            serverSocket.Bind(new IPEndPoint(0, 14000));
            serverSocket.Listen(1);
            MessageBox.Show("Serveri filloi te degjoj ne portin 14000");

            new Thread(() =>
            {
                accept = serverSocket.Accept();
                serverSocket.Close();

                while (true)
                {
                    try
                    {

                        byte[] buffer = new byte[2048];
                        int rec = accept.Receive(buffer, 0, buffer.Length, 0);

                        if (rec <= 0)
                        {
                            throw new SocketException();
                        }

                        Array.Resize(ref buffer, rec);

                        string data = Encoding.Default.GetString(buffer);

                        // data = decrypt(data);

                        string[] list = data.Split('.');

                        string controlFlow = list[list.Length - 1].Substring(0, 1);

                        if (controlFlow == "1")
                        {
                            var users = Users.KontrolloUsers();
                            if (users != null)
                            {
                                for (int i = 0; i < users.Count; i++)
                                {

                                    string u = users[i].Username;

                                    string SaltedHashi = Users.generateSaltedHash(list[1], users[i].Salt);

                                    if (u == list[0] && SaltedHashi == users[i].SaltedHash)
                                    {
                                        MessageBox.Show("MIRESEVINI " + users[i].Emri + " " + users[i].Mbiemri);
                                        XElement user = Users.getUserInfo(list[0]);
                                        string thisEmri = user.Element("emri").Value.ToString();
                                        string thisMbiemri = user.Element("mbiemri").Value.ToString();
                                        string thisInvoiceType = user.Element("invoiceType").Value.ToString();
                                        string thisYear = user.Element("year").Value.ToString();
                                        string thisMonth = user.Element("month").Value.ToString();
                                        string thisVleraEuro = user.Element("vleraEuro").Value.ToString();
                                        string thisShteti = user.Element("shteti").Value.ToString();

                                        var payload = new Dictionary<string, object>
                                        {
                                            { "name", thisEmri },
                                            { "surname", thisMbiemri },
                                            { "department", thisInvoiceType },
                                            { "year", thisYear },
                                            { "month", thisMonth },
                                            { "vleraEuro", thisVleraEuro },
                                            { "shteti", thisShteti },

                                        };


                                        byte[] data1 = ObjectToByteArray(payload);
                                        accept.Send(data1, 0, data1.Length, 0);






                                        break;

                                    }
                                    if (i == users.Count - 1)
                                    {
                                        MessageBox.Show("Keni gabuar username ose password");

                                    }
                                }
                                
                            }
                           
                        }
                        else if (Int32.Parse(list[list.Length - 1]) == 2)
                        {
                            string andi = Users.generateSalt();
                            string altini = Users.generateSaltedHash(list[3], andi);
                            Users user = new Users(list[0], list[1], list[2], altini, list[4], list[5], list[6], list[7], list[8], list[9], andi);

                            string strResultJson = JsonConvert.SerializeObject(user);
                            File.AppendAllText(@"Users.json", strResultJson);
                            File.AppendAllText(@"Users.json", ",");
                            Users.insert(list[0], list[1], list[2], list[3], altini, list[4], list[5], list[6], list[7], list[8], list[9], andi);

                        }
                        Invoke((MethodInvoker)delegate
                        {
                            Box.Items.Add("Eshte lidhur IPAddress dhe Port: ");
                            Box.Items.Add(accept.RemoteEndPoint);

                        });
                    }
                    catch (Exception ex)
                    {
                        
                        MessageBox.Show("Connection lost");
                        Application.Exit();
                    }
                }
            }).Start();
        }
      


        private string encrypt(string plaintext, string key, string iv)
        {
            objDes.Key = Convert.FromBase64String(key);
            objDes.IV = Encoding.Default.GetBytes(iv);
            objDes.Padding = PaddingMode.Zeros;
            objDes.Mode = CipherMode.CBC;


            byte[] bytePlaintexti = Encoding.UTF8.GetBytes(plaintext);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, objDes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(bytePlaintexti, 0, bytePlaintexti.Length);
            cs.Close();

            byte[] byteCiphertexti = ms.ToArray();

            return Convert.ToBase64String(byteCiphertexti);

        }
        private string decrypt(string ciphertext)
        {
            string[] info = ciphertext.Split('.');
            key = info[1];
            iv = info[0];

            objRsa = (RSACryptoServiceProvider)certifikata.PrivateKey;
            byte[] byteKey = objRsa.Decrypt(Convert.FromBase64String(key), true);

            key = Convert.ToBase64String(byteKey);
            objDes.Key = byteKey;
            objDes.IV = Encoding.Default.GetBytes(iv);
            objDes.Padding = PaddingMode.Zeros;
            objDes.Mode = CipherMode.CBC;

            byte[] byteCiphertexti = Convert.FromBase64String(info[2]);
            MemoryStream ms = new MemoryStream(byteCiphertexti);
            CryptoStream cs = new CryptoStream(ms, objDes.CreateDecryptor(), CryptoStreamMode.Read);

            byte[] byteTextiDekriptuar = new byte[ms.Length];
            cs.Read(byteTextiDekriptuar, 0, byteTextiDekriptuar.Length);
            cs.Close();

            string decryptedText = Encoding.UTF8.GetString(byteTextiDekriptuar);
            return decryptedText;
        }


        private void CertifikataBtn_Click(object sender, EventArgs e)
        {

           
            X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
             certificateStore.Open(OpenFlags.OpenExistingOnly);

             try
             {
                 X509Certificate2Collection certCollection = X509Certificate2UI.SelectFromCollection(
                     certificateStore.Certificates,"Certifikata", "Zgjedh certifikaten", X509SelectionFlag.SingleSelection);

                 certifikata = certCollection[0];
                 if (certifikata.HasPrivateKey)
                 {
                     MessageBox.Show("Certifikata e zgjedhur :  " +
                         certifikata.Subject);
                 }
                 else
                 {
                     MessageBox.Show("Certifikata e zgjedhur nuk permbane celes privat!");
                 }
             }
             catch (Exception ex)
             {

             }
             if (certifikata != null)
             {
                 byte[] byteCert = certifikata.Export(X509ContentType.Cert, "Altin1");
                 accept.Send(byteCert, 0, byteCert.Length, 0);
             }
        }
    }
    }

