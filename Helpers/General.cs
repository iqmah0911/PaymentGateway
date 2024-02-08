using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentGateway21052021.Areas.Invoices.Models;
using PaymentGateway21052021.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;

namespace PaymentGateway21052021.Helpers
{
    public class General
    {
        //public static async void callMethod()
        //{
        //    Task<int> task = Method1();
        //    Method2();
        //    int count = await task;
        //    Method3(count);
        //}


        //.......Logging Errors.............//



        private readonly static string _errorFolder;
        public IHttpContextAccessor _accessor;
        //private readonly static string _Ips;

        static General()
        {
            var subDirectory = Directory.GetCurrentDirectory();

            _errorFolder = Path.Combine(subDirectory + @"\\AppLog\\");

            if (!Directory.Exists(_errorFolder))
            {
                Directory.CreateDirectory(_errorFolder);
            }
        }

        //public static string strPath;
        //public static string[] currentFileName;
        //public static string[] incomingFolderName;
        //public static string[] outgoingFolderName;
        //public static string[] ExpectedFolderList;
        //public static string FromFileLocation;
        //public static string ToFilelocation;
        //public static string ArchiveName;
        //public static string expectedFolderList;
        //public static string Server;
        //public static string Subject;
        //public static string MessageBody;
        //public static string From;
        //public static string To;
        //public static string Cc;
        //public static long Counter;
        //public static string GeneralReportSummary;


        public static string Mask(string source, int start, int maskLength, char maskCharacter)
        {
            if (start > source.Length - 1)
            {
                throw new ArgumentException("Start position is greater than string length");
            }

            if (maskLength > source.Length)
            {
                throw new ArgumentException("Mask length is greater than string length");
            }

            if (start + maskLength > source.Length)
            {
                throw new ArgumentException("Start position and mask length imply more characters than are present");
            }

            string mask = new string(maskCharacter, maskLength);
            string unMaskStart = source.Substring(0, start);
            string unMaskEnd = source.Substring(start + maskLength, source.Length - maskLength);

            return unMaskStart + mask + unMaskEnd;
        }

        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                // aes.Padding = PaddingMode.None; //PKCS7


                // ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV); //CreateEncryptor(key, iv)
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);  //(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        //changes charles
                        //byte[] plainText = new byte[buffer.Length];
                        //int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);

                        //memoryStream.Close();
                        //cryptoStream.Close();

                        //return Encoding.Unicode.GetString(plainText, 0, decryptedCount);

                        //using (StreamReader sr =  new StreamReader(cryptoStream, Encoding.Unicode))
                        //{
                        //    var fresult = sr.ReadToEnd();
                        //    return fresult;
                        //}


                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }



                    }
                }
            }
        }

        public static string MaskString(string strToMask, int startIndex, string mask)
        {
            // Mask the mobile.
            // Usage: MaskMobile("13456789876", 3, "****") => "134****9876"
            if (string.IsNullOrEmpty(strToMask))
                return string.Empty;

            string result = strToMask;
            int starLengh = mask.Length;


            if (strToMask.Length >= startIndex)
            {
                result = strToMask.Insert(startIndex, mask);
                if (result.Length >= (startIndex + starLengh * 2))
                    result = result.Remove((startIndex + starLengh), starLengh);
                else
                    result = result.Remove((startIndex + starLengh), result.Length - (startIndex + starLengh));

            }

            return result;
        }

        static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }

        static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }


        public static string GetIPAddressLocal()
        {


            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            string IpAddress = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IpAddress = Convert.ToString(IP);
                }
            }
            return IpAddress;
        }

        public static string GetIPAddress()
        {
            var anInstanceofMyClass = new HttpContextAccessor();

            string IpAddress = anInstanceofMyClass.HttpContext.Connection.RemoteIpAddress.ToString();

            return IpAddress;
        }

        public static string GetCompleteExceptionMessage(Exception EX)
        {
            Exception exception = EX;
            string str = exception.Message;
            for (; exception.InnerException != null; exception = exception.InnerException)
                str = str + " because " + exception.InnerException.Message;
            return str;
        }


        protected static string BuildErrorMsg(Exception EX)
        {
            string str1 = "";
            string str2 = "";
            Exception baseException = EX.GetBaseException();
            if (EX == null)
                return "";
            string str3 = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
            string exceptionMessage = GetCompleteExceptionMessage(EX);
            if (baseException.TargetSite != (MethodBase)null)
                str1 = baseException.TargetSite.Name;
            if (baseException.StackTrace != null)
                str2 = EX.GetBaseException().StackTrace;
            return string.Format("\r\n\r\n[{0}]\r\n Subject: \t{1}\r\n Page Request: \t{2}\r\n Stack Trace : \t{3}", (object)str3, (object)exceptionMessage, (object)str1, (object)str2);
        }

        protected static string BuildStringErrorMsg(string lenghtErrorText)
        {
            string str = lenghtErrorText;
            if (lenghtErrorText == null)
                return "";
            return string.Format("\r\n\r\n[{0}]\r\n Stack Trace : \t{1}", new object[2]
            {
        (object) DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"),
        (object) str
            });
        }

        public static void LogToFile(Exception EX)
        {
            try
            {
                File.AppendAllText(_errorFolder + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", BuildErrorMsg(EX));
            }
            catch (NullReferenceException ex)
            {
                LogToFileLenght(ex.ToString());
            }
            catch (FileNotFoundException ex)
            {
                LogToFileLenght(ex.ToString());
            }
            catch (Exception ex)
            {
                LogToFile(ex);
            }
        }



        //public static void LogToFile(string message, string path = "")
        //{
        //    try
        //    {
        //        string _path = _errorFolder + (string.IsNullOrEmpty(path) ? "\\Others" : $"\\{path}\\");
        //        if (!Directory.Exists(_path))
        //        {
        //            Directory.CreateDirectory(_path);
        //        }
        //        File.AppendAllText(_path + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", message);
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        LogToFileLenght(ex.ToString());
        //    }
        //    catch (FileNotFoundException ex)
        //    {
        //        LogToFileLenght(ex.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        LogToFile(ex);
        //    }
        //}

        public static void LogToFileLenght(string lenghtErrorText)
        {
            try
            {
                File.AppendAllText(_errorFolder + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", BuildStringErrorMsg(lenghtErrorText));
            }
            catch (NullReferenceException ex)
            {
                LogToFile(ex);
            }
            catch (FileNotFoundException ex)
            {
                LogToFile(ex);
            }
            catch (Exception ex)
            {
                LogToFile(ex);
            }
        }



        //-------Logging Errors------------------//

        public static string GetHash(string[] param)
        {
            try
            {
                string finalHashedString = string.Empty;
                string hashedString = string.Empty;
                foreach (var prm in param)
                {
                    hashedString += prm;
                }
                using (SHA512Managed shaManager = new SHA512Managed())
                {
                    Byte[] encryptedString = shaManager.ComputeHash(Encoding.UTF8.GetBytes(hashedString));
                    finalHashedString = BitConverter.ToString(encryptedString).Replace("-", "").ToUpper();
                }
                return finalHashedString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // SHA-512 Hash Code Generator Method
        public static string Get512Hash(string strInput)
        {
            SHA512 sha512 = new SHA512CryptoServiceProvider();

            //provide the string in byte format to the ComputeHash method. 
            //This method returns the SHA-512 hash code in byte array
            byte[] arrHash = sha512.ComputeHash(Encoding.UTF8.GetBytes(strInput));

            // use a Stringbuilder to append the bytes from the array to create a SHA-512 hash code string.
            StringBuilder sbHash = new StringBuilder();

            // Loop through byte array of the hashed code and format each byte as a hexadecimal code.
            for (int i = 0; i < arrHash.Length; i++)
            {
                sbHash.Append(arrHash[i].ToString("x2"));
            }

            // Return the hexadecimal SHA-512 hash code string.
            return sbHash.ToString();
        }

        public static string GetRechargeHash(string username, string password)
        {
            try
            {
                string finalHashedString = string.Empty;
                string hashedString = string.Empty;

                string HashKey = "CLOUDRELLERAGENT00001554";

                hashedString = username + password + HashKey;

                using (SHA512Managed shaManager = new SHA512Managed())
                {
                    Byte[] encryptedString = shaManager.ComputeHash(Encoding.UTF8.GetBytes(hashedString));
                    finalHashedString = BitConverter.ToString(encryptedString).Replace("-", "").ToUpper();
                }
                return finalHashedString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string MakeRequest(string RequestURL, string RequestString, string RequestMethod)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(RequestURL) as HttpWebRequest;
                request.Method = RequestMethod;
                request.ContentType = "text";
                request.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR1.0.3705;)");
                //request.Headers.Add("Content-Type", "text/xml");
                //    byte[] bytes = Encoding.UTF8.GetBytes(RequestString);
                //    Stream requestStream = request.GetRequestStream();
                //    requestStream.Write(bytes, 0, bytes.Length);
                //    requestStream.Close();
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    return new StreamReader(response.GetResponseStream()).ReadToEnd();
                }

            }
            catch (Exception exception)
            {
                //WriteLog(exception.Message + exception.StackTrace, @"c:\Logs\Errors\");
                return "500";
            }
        }

        public static string MakeVFDRequest(string RequestURL, string RequestString = null, string RequestMethod = null, string token = null, string bodyRequest = null)
        {
            string baseRequestURL = "";
            if (RequestString == null)
            {
                baseRequestURL = RequestURL;
            }
            else
            {
                baseRequestURL = RequestURL + RequestString;
            }
            var client = new RestClient(baseRequestURL);
            RestRequest request = new RestRequest();

            if (RequestMethod.ToLower() == "get")
            {
                request = new RestRequest(Method.GET);
            }
            else if (RequestMethod.ToLower() == "post")
            {
                request = new RestRequest(Method.POST);
            }

            if (token != null && token != "")
            {
                request.AddHeader("Authorization", "Bearer " + token);
            }

            if (bodyRequest != null && bodyRequest != "")
            {
                request.AddParameter("application/json", bodyRequest, ParameterType.RequestBody);
            }

            request.AddHeader("Accept", "application/json");

            var response = client.Execute(request);

            var content = response.Content;

            return content;

        }

        public static string GetInvoiceRef(string transactionRef)
        {
            string refNumber = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16).ToUpper();//"0000000000000000";
            //string refNumber = "00000000";// 00000000";
            //refNumber.Re
            if (!string.IsNullOrEmpty(transactionRef))
            {
                return refNumber.Substring(0, (refNumber.Length - transactionRef.ToString().Length) - 1);
            }
            return $"INV{refNumber}N".ToUpper();
        }

        public static DataTable JsonToDataTable(string jsonString)
        {
            var jsonLinq = JObject.Parse(jsonString);

            // Find the first array using Linq  
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types  
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }
                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }


        public static long randomNumber()
        {
            var random = new Random();
            long randomNumber = random.Next(1, 999999999) + random.Next(1, 999999999) + random.Next(1, 999999999);
            return randomNumber;
        }


        public static string GetMonthInWord(int month)
        {
            var monthNames = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            int getMonth = month;

            return $"{monthNames[getMonth - 1] }";
        }

        public static async Task<string> ServiceRequest(string RequestURL, string RequestString = null, string RequestMethod = null, string token = null, string bodyRequest = null)
        {

            string baseRequestURL = "";
            if (RequestString == null)
            {
                baseRequestURL = RequestURL;
            }
            else
            {
                baseRequestURL = RequestURL + RequestString;
            }

            var client = new HttpClient();
            //HttpResponseMessage response = new HttpResponseMessage();
            HttpMethod typemethod = new HttpMethod(RequestMethod.ToLower());
            HttpRequestMessage request = new HttpRequestMessage();
            var content = "";

            // Add Bearer token to header   
            if (token != null && token != "")
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Add request body 
            if (bodyRequest != null && bodyRequest != "")
            {
                Uri uri = new Uri(baseRequestURL); ;//client.GetAsync(baseRequestURL);
                request = new HttpRequestMessage
                {
                    Method = typemethod,
                    RequestUri = uri,
                    Content = new StringContent(bodyRequest),
                };
            }
            else
            {
                Uri uri = new Uri(baseRequestURL);
                request = new HttpRequestMessage
                {
                    Method = typemethod,
                    RequestUri = uri,
                };
            }
            var response = await client.SendAsync(request).ConfigureAwait(false);
            var responseInfo = await response.Content.ReadAsStringAsync();
            //response = await client.SendAsync(request).Result;
            content = responseInfo;//await response.Content.ReadAsStringAsync().Result;//gcontent.ToString();

            return content;

        }


    }
}
