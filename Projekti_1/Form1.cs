using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Base64EncoderDecoder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void encodeButton_Click(object sender, EventArgs e)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(plaintextTextBox.Text);
            Base64_Encoder myEncoder = new Base64_Encoder(data);
            StringBuilder sb = new StringBuilder();

            sb.Append(myEncoder.Encode());

            encodedTextBox.Text = sb.ToString();
        }

        private void decodeButton_Click(object sender, EventArgs e)
        {
            char[] data = encodedTextBox.Text.ToCharArray();
            Base64_Decoder myDecoder = new Base64_Decoder(data);
            StringBuilder sb = new StringBuilder();

            byte[] temp = myDecoder.Decode();
            sb.Append(System.Text.UTF8Encoding.UTF8.GetChars(temp));

            decodedTextBox.Text = sb.ToString();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            plaintextTextBox.Clear();
            encodedTextBox.Clear();
            decodedTextBox.Clear();
        }

        private void Forma_Load(object sender, EventArgs e)
        {

        }
    }
}
