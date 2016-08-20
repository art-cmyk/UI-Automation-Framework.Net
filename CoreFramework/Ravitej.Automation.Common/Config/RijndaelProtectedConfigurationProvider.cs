using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Ravitej.Automation.Common.Config
{
    public class RijndaelProtectedConfigurationProvider : ProtectedConfigurationProvider
    {
        private readonly SymmetricAlgorithm _algorithm = new RijndaelManaged();

        private string _name;

        public string KeyFilePath
        {
            get;
            private set;
        }

        public override string Name => this._name;

        public override void Initialize(string sName, NameValueCollection dictConfig)
        {
            this._name = sName;
            this.KeyFilePath = dictConfig["keyContainerName"];
            this.ReadKey(this.KeyFilePath);
        }

        public override XmlNode Encrypt(XmlNode node)
        {
            var sEncryptedData = this.EncryptString(node.OuterXml);
            var xmlDoc = new XmlDocument {PreserveWhitespace = true};
            xmlDoc.LoadXml("<EncryptedData>" + sEncryptedData + "</EncryptedData>");
            return xmlDoc.DocumentElement;
        }

        public override XmlNode Decrypt(XmlNode encryptedNode)
        {
            var sDecryptedData = this.DecryptString(encryptedNode.InnerText);
            var xmlDoc = new XmlDocument {PreserveWhitespace = true};
            xmlDoc.LoadXml(sDecryptedData);
            return xmlDoc.DocumentElement;
        }

        public void CreateKey(string sFilePath)
        {
            this._algorithm.GenerateKey();
            this._algorithm.GenerateIV();
            using (var oWriter = new StreamWriter(sFilePath, false))
            {
                oWriter.WriteLine(this.ByteToHex(this._algorithm.Key));
                oWriter.WriteLine(this.ByteToHex(this._algorithm.IV));
            }
        }

        private string EncryptString(string sEncryptValue)
        {
            var abBytes = Encoding.Unicode.GetBytes(sEncryptValue);
            var oTransform = this._algorithm.CreateEncryptor();
            string result;
            using (var oMemoryStream = new MemoryStream())
            {
                using (var oCryptoStream = new CryptoStream(oMemoryStream, oTransform, CryptoStreamMode.Write))
                {
                    oCryptoStream.Write(abBytes, 0, abBytes.Length);
                    oCryptoStream.FlushFinalBlock();
                    result = Convert.ToBase64String(oMemoryStream.ToArray());
                }
            }
            return result;
        }

        private string DecryptString(string sEncryptedValue)
        {
            var abBytes = Convert.FromBase64String(sEncryptedValue);
            var oTransform = this._algorithm.CreateDecryptor();
            string @string;
            using (var oMemoryStream = new MemoryStream())
            {
                using (var oCryptoStream = new CryptoStream(oMemoryStream, oTransform, CryptoStreamMode.Write))
                {
                    oCryptoStream.Write(abBytes, 0, abBytes.Length);
                    oCryptoStream.FlushFinalBlock();
                    @string = Encoding.Unicode.GetString(oMemoryStream.ToArray());
                }
            }
            return @string;
        }

        private void ReadKey(string sFilePath)
        {
            using (var oReader = new StreamReader(sFilePath))
            {
                this._algorithm.Key = this.HexToByte(oReader.ReadLine());
                this._algorithm.IV = this.HexToByte(oReader.ReadLine());
            }
        }

        private string ByteToHex(byte[] abBytes)
        {
            return string.Join(string.Empty, from b in abBytes
                                             select b.ToString("X2"));
        }

        private byte[] HexToByte(string sHex)
        {
            var abReturnBytes = new byte[sHex.Length / 2];
            for (var i = 0; i < abReturnBytes.Length; i++)
            {
                abReturnBytes[i] = Convert.ToByte(sHex.Substring(i * 2, 2), 16);
            }
            return abReturnBytes;
        }
    }
}
