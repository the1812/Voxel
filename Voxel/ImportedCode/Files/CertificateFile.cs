using System;
using System.IO;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Ace.Files
{
    /// <summary>
    /// 表示证书的签名算法
    /// </summary>
    public enum SignatureAlgorithm
    {
        /// <summary>
        /// 使用sha1算法
        /// </summary>
        Sha1,
        /// <summary>
        /// 使用sha256算法
        /// </summary>
        Sha256,
    }
    /// <summary>
    /// <para>表示一个用SHA1或SHA256算法签名的证书(.cer)文件</para>
    /// <para>注：不支持.pfx文件</para>
    /// </summary>
    public class CertificateFile : FileBase
    {
        /// <summary>
        /// 使用二进制数据初始化证书文件
        /// </summary>
        /// <param name="data">二进制数据</param>
        public CertificateFile(byte[] data) : base("")
        {
            cer = new X509Certificate2(data);
        }
        /// <summary>
        /// 从路径初始化证书文件
        /// </summary>
        /// <param name="path">证书文件的路径</param>
        public CertificateFile(string path) : base(path)
        {
            if (!path.EndsWith(".cer"))
            {
                throw new ArgumentException("必须使用.cer文件初始化。");
            }
        }
        private X509Certificate2 cer;
        /// <summary>
        /// 使用X509Certificate2对象初始化证书文件
        /// </summary>
        /// <param name="cer">X509Certificate2对象</param>
        public CertificateFile(X509Certificate2 cer) : base("")
        {
            this.cer = cer;
        }
        /// <summary>
        /// 获取对应的X509Certificate2对象
        /// </summary>
        public X509Certificate2 Certificate => cer;
        /// <summary>
        /// 获取证书的序列号
        /// </summary>
        public string SerialNumber => cer.SerialNumber;
        /// <summary>
        /// 获取证书的通用名称
        /// </summary>
        public string CommonName => cer.GetNameInfo(X509NameType.DnsName, false);
        /// <summary>
        /// 获取证书的签名算法
        /// </summary>
        public SignatureAlgorithm SignatureAlgorithm => cer.SignatureAlgorithm.FriendlyName.Contains("256") ? SignatureAlgorithm.Sha256 : SignatureAlgorithm.Sha1;
        private X509Store getDisallowedStore()
        {
            X509Store store = new X509Store(StoreName.Disallowed, StoreLocation.LocalMachine);
            store.Open(OpenFlags.MaxAllowed);
            return store;
        }
        /// <summary>
        /// 获取指示证书是否不允许的值
        /// </summary>
        public bool IsDisallowed
        {
            get
            {
                var store = getDisallowedStore();
                foreach (var cer in store.Certificates)
                {
                    CertificateFile file = new CertificateFile(cer);
                    if (file.Equals(this))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// 将证书设为不允许
        /// </summary>
        public void Disallow()
        {
            var store = getDisallowedStore();
            store.Add(cer ?? throw new NullReferenceException());
        }
        /// <summary>
        /// 将证书设为允许
        /// </summary>
        public void Allow()
        {
            var store = getDisallowedStore();
            store.Remove(cer ?? throw new NullReferenceException());
        }
        /// <summary>
        /// 将内容写回文件
        /// </summary>
        public override void Flush()
        {
            var data = cer?.Export(X509ContentType.Cert) ?? throw new NullReferenceException();
            if (string.IsNullOrEmpty(Path)) throw new IOException();
            using (var stream = new FileStream(Path, FileMode.Create))
            {
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
        }
        /// <summary>
        /// 读取文件内容
        /// </summary>
        public override void Read()
        {
            if (File.Exists(Path))
            {
                cer = new X509Certificate2(Path);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is CertificateFile file)
            {
                return file.SerialNumber == SerialNumber
                    && file.SignatureAlgorithm == SignatureAlgorithm;
            }
            else
                return false;
        }
    }
}
