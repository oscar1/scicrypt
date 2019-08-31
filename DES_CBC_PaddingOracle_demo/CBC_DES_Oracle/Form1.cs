using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private byte[] hextobytes(string hexstr)
        {
            var arr = new byte[hexstr.Length / 2];
            for (var i = 0; i < arr.Length; i++)
                arr[i] = (byte)Convert.ToInt32(hexstr.Substring(i * 2, 2), 16);
            return arr;
        }

        private DESCryptoServiceProvider getProvider()
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            //provider.BlockSize = 8;
            provider.Key = System.Text.Encoding.ASCII.GetBytes(keyBox.Text);
            provider.IV = hextobytes(IVBox.Text);
            provider.Mode = CipherMode.CBC;
            return provider;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DESCryptoServiceProvider provider = getProvider();
            
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, provider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(plainBox.Text);
                    }
                    //cypherBox.Text = Convert.ToBase64String( msEncrypt.ToArray());
                    cypherBox.Text = BitConverter.ToString(msEncrypt.ToArray()).Replace("-", "");
                    label5.Text = "Length cipher: " + cypherBox.Text.Length/2;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        
            DESCryptoServiceProvider provider = getProvider();

           // using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cypherBox.Text)))
            using (MemoryStream msDecrypt = new MemoryStream(hextobytes(cypherBox.Text)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, provider.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the  decrypting stream 
                        // and place them in a string.
                        try
                        {
                            plainBox.Text = srDecrypt.ReadToEnd();
                        } catch (Exception exc) // fix thus handler? dont rememebr
                        {
                            MessageBox.Show(exc.Message);
                        }
                    }
                }
            }
        }

        private string decrypt(byte[] iv, byte[]ciphertext)
        {
            string retval;
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            //provider.BlockSize = 8;
            provider.Key = System.Text.Encoding.ASCII.GetBytes(keyBox.Text);
            provider.IV = iv;
            provider.Mode = CipherMode.CBC;
            using (MemoryStream msDecrypt = new MemoryStream(ciphertext))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, provider.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the  decrypting stream 
                            // and place them in a string.

                           retval = srDecrypt.ReadToEnd();
                            
                        }
                    
                }
            }
            return retval;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 
            string h = "0000000000000000";
            byte[] origiv = hextobytes(IVBox.Text);
            string plaintext = "";
            progressBar1.Minimum = 0;
            progressBar1.Maximum = cypherBox.Text.Length / 2;
            progressBar1.Step = 1;

            for (int n = 0; n < cypherBox.Text.Length / 16; n++) // loop through 8 byte blocks
            {
                string block = cypherBox.Text.Substring(n*16, 16);
                byte[] intermediates = hextobytes(h);
                byte[] iv = hextobytes(h);
                string plainblock = "";

                for (int k = 7; k >= 0; k--) // loop through each byte
                {
                    byte padval = (byte)(8 - k);

                    for (int i = 0; i < 256; i++) // loop through 256 vals until no exception thrown
                    {
                        iv[k] = (byte)i;
                        try
                        {
                            decrypt(iv, hextobytes(block)); // throws if padding incorrect, here we exploit that knowlegde
                            // 200 OK
                            label5.Text = "No error at " + i;
                            intermediates[k] = (byte)(i ^ padval);
                            
                            plainblock = ((char)(intermediates[k] ^ origiv[k])).ToString() + plainblock;
                            textBox1.Text = plainblock;
 
                            //byte nextpad = (byte)(interm ^ (8-k+1));

                            for (int j = 0; j < padval; j++)
                            {
                                iv[7 - j] = (byte)((padval + 1) ^ intermediates[7 - j]);
                            }
                            progressBar1.Value += 1;
                            break;
                        }
                        catch (CryptographicException exc)
                        {
                            // 500 Internal Server Error
                            label5.Text = "Exception" + exc.Message + " at " + i;

                        }
                        Application.DoEvents();
                    }
                }
                origiv = hextobytes(block);
                plaintext += plainblock;
            }
            MessageBox.Show("Plaintext: " + plaintext);
        }
    }
}
