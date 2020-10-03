using HtmlAgilityPack;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using HtmlDocument = System.Windows.Forms.HtmlDocument;
using System.Runtime.InteropServices;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Media;
using System.Reflection;

namespace BotApp
{    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //        [DllImport("user32")]
        //       public static extern int EnumWindows(CallBack x, int y);
        private string m_strProduct;
        private string m_strColor;
        private string m_strJSON_Temp;
        private string m_strJSON;
        private string m_select1index;
        private string m_strSelect2;
        private string[] m_strURLs;

        private string strIndex_product_id, strIndex_product_id_temp;
        private string m_strIndex_Temp;


        private bool m_Flag;
        private bool m_LoginFlag;
        private bool m_LoginValidationFlag;
        private bool m_LogoutFlagT;
        private bool m_LogoutFlagH;
        private bool m_SendFlag;
        private bool m_StartFlag;
        private bool m_CartFlagH, m_CartFlagT;
        private bool m_CheckState, m_CheckStateH;
        private bool m_LoginState;
        private bool m_URLValid;
        private bool m_CartPage;
        private bool m_CheckoutFlag1;
        private bool m_CheckoutFlag2;
        private int iNumber;
        private int iProduct, jProduct;
        private string m_strElement;
        private bool m_ErrorFlag;
        private bool m_CCH, m_CCT;
        private bool m_ProductH, m_ProductT;
        private bool m_Submit;
        private bool m_BugFlag;
        private bool m_dataValidation;
        private bool m_TimeSet;
        private bool m_AutoProduct;
        private bool m_AutoProductState;
        private bool m_Info;
        private bool m_CheckCard;
        private bool m_CardFlag;
        private bool m_CheckPayMent;

        private Dictionary<string, string> dcolor;
        private Dictionary<string, string> dsize;

//        private string[][] m_Product;
        private string m_strStatus;
        private int m_Count;
        private int m_TimeCount;
        private int m_Second;
        Product m_lvitem;
        public MainWindow()
        {

            InitializeComponent();

            DispatcherTimer dtClockTime = new DispatcherTimer();

            dtClockTime.Interval = new TimeSpan(0, 0, 1); //in Hour, Minutes, Second.
            dtClockTime.Tick += TimerTick;

            dtClockTime.Start();

            httpRequest.Navigate("https://www.wtaps.com/mypage/login");
            m_Flag = true;

            //httpRequest.ScriptErrorsSuppressed = true;

            urlPath.Text = "https://www.wtaps.com/products/detail/445";

            m_Info = false;

            m_TimeSet = false;
            m_AutoProduct = false;
            m_AutoProductState = false;

            m_LoginState = false;
            m_LoginFlag = false;
            m_LoginValidationFlag = false;
            m_LogoutFlagT = false;
            m_LogoutFlagH = false;
            m_SendFlag = false;
            m_StartFlag = false;
            m_CartFlagH = false;
            m_CartFlagT = false;
            m_CheckState = false;
            m_CheckStateH = false;

            m_URLValid = false;
            m_CartPage = false;
            m_CardFlag = false;
            m_ErrorFlag = false;

            m_ProductH = false;
            m_ProductT = false;
           
            m_Submit = false;
            m_BugFlag = true;
            m_dataValidation = true;

            m_CheckCard = true;
            m_CheckPayMent = false;

            strIndex_product_id_temp = "//";

            iNumber = 1;
            iProduct = 0;
            jProduct = 0;

            dcolor = new Dictionary<string, string>();
            dsize = new Dictionary<string, string>();

            m_CheckoutFlag1 = false;
            m_CheckoutFlag2 = false;

            cmbPayMethod.Items.Add("クレジットカード決済");
            cmbPayMethod.Items.Add("代金引換");

            cmbPayMethod.SelectedIndex = 0;

            for (int iMonth = 1; iMonth < 13; iMonth++)
                cmbMonth.Items.Add(iMonth.ToString());

            for (int iYear = 2020; iYear < 2040; iYear++)
                cmbYear.Items.Add(iYear.ToString());
            

            string[] strColor = { "" };
            string[] strSize = { "" };

            if (File.Exists(".\\data\\color.txt"))
            {
                strColor = File.ReadAllLines(".\\data\\color.txt");
            }

            if (File.Exists(".\\data\\size.txt"))
            {
                strSize = File.ReadAllLines(".\\data\\size.txt");
            }

            for (int i = 0; i < strColor.Length; i++)
                cmbColor.Items.Add(strColor[i]);

            for (int j = 0; j < strSize.Length; j++)
                cmbSize.Items.Add(strSize[j]);

            m_CCH = false;

            
            m_Count = 1;
            m_TimeCount = 0;
            m_Second = 5;

            if (File.Exists(".\\data\\information.txt"))
            {
                File.Delete(".\\data\\information.txt");
            }

            InformationFile();
        }

        private void InformationFile()
        {
            DateTime.Now.ToLocalTime().ToString();

            File.AppendAllText(".\\data\\information.txt", DateTime.Now.ToLocalTime().ToString() + "\t " + m_strStatus + "  \n");
            /*using (StreamWriter writer = new StreamWriter("information.txt"))
             {

                 writer.Write("Word ");
                 writer.WriteLine("word 2");
                 writer.WriteLine("Line");
             }*/
        }

        string strColor;
        public void GetData()
        {

            if (httpRequest.Document != null)
            {
                //m_StartFlag = false;
                m_ProductT = false;
                mshtml.HTMLDocument document = (mshtml.HTMLDocument)httpRequest.Document;

                if (document.title == "ページがみつかりません。")
                {
                    gif.Visibility = Visibility.Visible;

                    iProduct = 0;
                    jProduct = 0;

                    m_ProductH = true;
                    //m_ProductT = true;
                    m_StartFlag = false;

                    btnProducts.IsEnabled = false;
                    btnCart.IsEnabled = false;

                    btnLogout.IsEnabled = false;
                    m_BugFlag = true;

                    httpRequest.Navigate(urlPath.Text);
                    //System.Windows.MessageBox.Show("The URL is invalid. Input another URL.", "Information");
                    return;
                }

                mshtml.IHTMLElementCollection aTags = document.getElementsByTagName("a");
                foreach (mshtml.IHTMLElement element in aTags)
                {
                    if (element.className == "delivary_mail_btn")
                    {
                        
                        //httpRequest.Refresh();

                        gif.Visibility = Visibility.Visible;

                        iProduct = 0;
                        jProduct = 0;

                        m_ProductH = true;
                        //m_ProductT = true;
                        m_StartFlag = false;

                        btnProducts.IsEnabled = false;
                        btnCart.IsEnabled = false;

                        btnLogout.IsEnabled = false;
                        m_BugFlag = true;

                        httpRequest.Navigate(urlPath.Text);
                        //System.Windows.MessageBox.Show("The URL is invalid. Input another URL.", "Information");
                        return;
                    }
                }
                
                m_strProduct = document.title;
                m_BugFlag = true;

                /******************** Get JSON Datas*********************/

                mshtml.IHTMLElementCollection scripts = document.getElementsByTagName("script");

                foreach (mshtml.IHTMLElement element in scripts)
                {
                    
                    string strTemp = "eccube.classCategories";
                    string strElement = element.innerHTML;
                    //System.Windows.MessageBox.Show(strElement);
                    m_strElement = strElement;
                    if (strElement == null) continue;

                    if (strElement.Contains(strTemp))
                    {
                        m_strJSON_Temp = strElement.Substring(30, strElement.IndexOf(';') - 30);
                        
                        break;
                    }
                }

                if (m_strJSON_Temp == null)
                {
                    m_ProductT = true;
                    
                    return;
                }





                JObject json = JObject.Parse(m_strJSON_Temp);
                
                m_strJSON_Temp = null;
                /***********************************************************/
                mshtml.IHTMLElementCollection options = document.getElementsByTagName("option");
                
                var select1 = document.getElementById("classcategory_id1");
                var select2 = document.getElementById("classcategory_id2");

                mshtml.HTMLSelectElement cbProyectos = select1 as mshtml.HTMLSelectElement;
                //mshtml.HTMLSelectElement cbProyectos2 = select2 as mshtml.HTMLSelectElement;
                //System.Windows.MessageBox.Show(cbProyectos2.length.ToString());

                if (cbProyectos == null)
                {
//                    gif.Visibility = Visibility.Hidden;
//                    m_strStatus = "The URL is invalid. Try again.";
//                    //System.Windows.MessageBox.Show("The URL is invalid. Try again.");

                    gif.Visibility = Visibility.Visible;

                    iProduct = 0;
                    jProduct = 0;

                    m_ProductH = true;
                    //m_ProductT = true;
                    m_StartFlag = false;

                    btnProducts.IsEnabled = false;
                    btnCart.IsEnabled = false;

                    btnLogout.IsEnabled = false;
                    m_BugFlag = true;

                    httpRequest.Navigate(urlPath.Text);
                    //System.Windows.MessageBox.Show("The URL is invalid. Input another URL.", "Information");
                    return;



                    //btnLogout.IsEnabled = true;
                    btnLogin.IsEnabled = false;
                    btnProducts.IsEnabled = true;

//                    btnCart.IsEnabled = true;

                    m_URLValid = true;
                    return;
                }

                

                int total = cbProyectos.length; // 0 return/

                if (cmbColor.Text == "" && cmbSize.Text == "")
                {
                    for (var i = 0; i < total; i++)
                    {
                        cbProyectos.selectedIndex = i;
                        if (cbProyectos.value.Contains("__unselected"))
                        {
                            continue;
                        }
                        //System.Windows.MessageBox.Show(document.getElementById("classcategory_id1").getAttribute("value"));

                        foreach (mshtml.IHTMLElement option in options)
                        {
                            //System.Windows.MessageBox.Show(cbProyectos.innerText);
                            string strTEST = cbProyectos.innerText;
                            //if (cmbColor.Text == option.innerText)
                            //{
                                if (option.getAttribute("value") == cbProyectos.value)
                                {
                                    strColor = option.innerText;
                                break;
                                    //System.Windows.MessageBox.Show(option.innerText);
                                }
                            //}
                        }

                        //if (strColor != cmbColor.Text)
                        //    continue;

                        //document.getElementById("classcategory_id1").setAttribute("value", cbProyectos.value);

                        //cbProyectos.setAttribute("value", cbProyectos.value);


                        m_select1index = Convert.ToString(cbProyectos.value);

                        //System.Windows.MessageBox.Show(Convert.ToString(cbProyectos.value));
                        ////////////////////////////////////////////////////////
                        JToken select2Tokens = json.GetValue(m_select1index);

                        foreach (JToken selectToken in select2Tokens)
                        {

                            foreach (JToken select2_Json in selectToken)
                            {

                                string strIndex_Temp = (string)select2_Json["name"];
                                string strIndex_value = (string)select2_Json["classcategory_id2"];

                                m_strSelect2 = strIndex_value;
                                if (/*strColor == cmbColor.Text && strIndex_Temp == cmbSize.Text*/strIndex_Temp != "Please select." && !strIndex_Temp.Contains("(SOLD OUT)"))
                                {
                                    //m_strIndex_Temp = strIndex_Temp;
                                    //m_strColor = strColor;

                                    //strIndex_product_id = (string)select2_Json["product_class_id"];
                                    dcolor.Add((string)select2_Json["product_class_id"], strColor);
                                    dsize.Add((string)select2_Json["product_class_id"], strIndex_Temp);
                                    if (strIndex_Temp.Contains("(SOLD OUT)"))
                                    {
                                        listData.Items.Add(new Product() { no = iNumber, product = m_strProduct, product_class_id = (string)select2_Json["product_class_id"], color = strColor, size = strIndex_Temp, status = "SOLD OUT", select1 = m_select1index, select2 = m_strSelect2 });
                                        m_dataValidation = true;
                                    }
                                    else
                                    {
                                        listData.Items.Add(new Product() { no = iNumber, product = m_strProduct, product_class_id = (string)select2_Json["product_class_id"], color = strColor, size = strIndex_Temp, status = "Waiting for schedule", select1 = m_select1index, select2 = m_strSelect2 });
                                        m_dataValidation = false;
                                    }
                                    strColor = "";
                                    iNumber++;
                                    //                                iProduct++;
                                    
                                    break;
                                }
                            }

                            if (strColor == "")
                                break;
                        }
                        if (strColor == "")
                           break;
                    }
                }
                else {
                    for (var i = 0; i < total; i++)
                    {
                        cbProyectos.selectedIndex = i;
                        if (cbProyectos.value.Contains("__unselected"))
                        {
                            continue;
                        }
                        //System.Windows.MessageBox.Show(document.getElementById("classcategory_id1").getAttribute("value"));

                        foreach (mshtml.IHTMLElement option in options)
                        {
                            //System.Windows.MessageBox.Show(cbProyectos.innerText);
                            //System.Windows.MessageBox.Show(cbProyectos.value);
                            if (cmbColor.Text == option.innerText)
                            {
                                if (option.getAttribute("value") == cbProyectos.value)
                                {
                                    strColor = option.innerText;
                                    break;
                                    //System.Windows.MessageBox.Show(option.innerText);
                                }
                            }
                        }

                        if (strColor != cmbColor.Text)
                            continue;

                        //document.getElementById("classcategory_id1").setAttribute("value", cbProyectos.value);

                        //cbProyectos.setAttribute("value", cbProyectos.value);


                        m_select1index = Convert.ToString(cbProyectos.value);

                        //System.Windows.MessageBox.Show(Convert.ToString(cbProyectos.value));
                        ////////////////////////////////////////////////////////
                        JToken select2Tokens = json.GetValue(m_select1index);
                        
                        foreach (JToken selectToken in select2Tokens)
                        {

                            foreach (JToken select2_Json in selectToken)
                            {

                                string strIndex_Temp = (string)select2_Json["name"];
                                string strIndex_value = (string)select2_Json["classcategory_id2"];

                                m_strSelect2 = strIndex_value;
                                if (strColor == cmbColor.Text && strIndex_Temp == cmbSize.Text/*strIndex_Temp != "Please select." && !strIndex_Temp.Contains("(SOLD OUT)")*/)
                                {
                                    //m_strIndex_Temp = strIndex_Temp;
                                    //m_strColor = strColor;

                                    //strIndex_product_id = (string)select2_Json["product_class_id"];
                                    dcolor.Add((string)select2_Json["product_class_id"], strColor);
                                    dsize.Add((string)select2_Json["product_class_id"], strIndex_Temp);
                                    listData.Items.Add(new Product() { no = iNumber, product = m_strProduct, product_class_id = (string)select2_Json["product_class_id"], color = strColor, size = strIndex_Temp, status = "Waiting for schedule", select1 = m_select1index, select2 = m_strSelect2 });
                                    strColor = "";
                                    iNumber++;
                                    //                                iProduct++;
                                    m_dataValidation = false;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }

                
            }

            if(m_dataValidation)
            {
                m_StartFlag = false;
                gif.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                btnProducts.IsEnabled = true;
                btnLogout.IsEnabled = true;
                m_dataValidation = true;
                System.Windows.MessageBox.Show("The product doesn't exist. Perhaps the product may be sold out.", "Information");
                return;
            }

            if(listData.Items.Count == 0)
            {
                m_ProductT = true;
            }

            
//            btnProducts.IsEnabled = true;
//            btnCart.IsEnabled = true;
//            btnDelete.Visibility = Visibility.Visible;

            iProduct = listData.Items.Count;

            btnCart.IsEnabled = false;
            btnProducts.IsEnabled = false;
            btnDelete.Visibility = Visibility.Hidden;
            gif.Visibility = Visibility.Visible;

            //m_SendFlag = true;
            m_StartFlag = true;
        }

        public static bool Report(int hwnd, int lParam)
        {

            return true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (httpRequest.Document != null)
            {

                if (btnLogin.IsVisible)
                {
                    if (email.Text == "" || !email.Text.Contains("@") || !email.Text.Contains("."))
                    {
                        System.Windows.MessageBox.Show("Invalid Email. Try again.", "Information");
                        email.Text = "";
                        email.Focus();
                        return;
                    }

                    if (pass.Text == "")
                    {
                        System.Windows.MessageBox.Show("Input Password. Try again.", "Information");
                        pass.Focus();
                        return;
                    }


                    mshtml.HTMLDocument document = (mshtml.HTMLDocument)httpRequest.Document;
                    var btnSubmits = document.getElementsByTagName("button");
                    //document.getElementsByTagName("button")[0].click();
                    //var dd = document.getElementById("login_pass").getAttribute("value", 0);
                    //
                    foreach (mshtml.IHTMLElement element in btnSubmits)
                    {
                        //System.Windows.Forms.MessageBox.Show("1234567890    1234567890");
                        element.click();
                    }
                    ///////////////////////////////////////////////////
                    ////////////////////////////////////////////////////
                    gif.Visibility = Visibility.Visible;
                    btnLogin.IsEnabled = false;
                    m_LoginFlag = true;

                    btnProducts.IsEnabled = false;

                    chk_Time.IsEnabled = false;
 //                   btnCart.IsEnabled = false;
                    //btnLogin.Visibility = Visibility.Hidden;
                    //btnLogout.Visibility = Visibility.Visible;
               }
               else
               {
//                     httpRequest.Navigate("http://www.wtaps.com/logout");
//                     btnLogin.Visibility = Visibility.Visible;
//                     btnLogout.Visibility = Visibility.Hidden;
               }

            }
            else
            {
                System.Windows.MessageBox.Show("Connection is failed. Tray again.", "Information");
                email.Text = "";
                pass.Text = "";
                return;
            }

        }

        private void BtnImportURL_Click(object sender, RoutedEventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            //System.Windows.MessageBox.Show(fileContent, "File Content at path: " + filePath);
            file_Path.Text = filePath;

            m_strURLs = fileContent.Split('\n');
            
            httpRequest.Navigate(m_strURLs[0]);
            
            //System.Threading.Thread.Sleep(5000);
            //AddCart();
            //var temp = httpRequest.Source.ToString();
        }

        private void Email_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //System.Windows.MessageBox.Show(sender.ToString());
            //System.Windows.MessageBox.Show(e.ToString());

            if (sender.ToString().Length == 31)
                return;

            string _strEmail = sender.ToString().Substring(32);
            _strEmail.Remove(0, 1);
            if (_strEmail.Length == 0)
                _strEmail = "";
            if (httpRequest.Document != null)
            {
                mshtml.HTMLDocument document = (mshtml.HTMLDocument)httpRequest.Document;
                var inputs = document.getElementsByTagName("input");


                int count = 0;
                foreach (mshtml.IHTMLElement element in inputs)
                {
                    if (count == 0)
                    {
                        //element.setAttribute("value", email.Text);
                        //System.Windows.Forms.MessageBox.Show("1234567890");
                        //element.onkeypress();
                        element.setAttribute("value", _strEmail.Remove(0,1));
                        break;
                    }
                    count++;
                }
                
                
                /*               var btnSubmits = document.getElementsByTagName("button");
                               //document.getElementsByTagName("button")[0].click();
                               foreach (mshtml.IHTMLElement element in btnSubmits)
                               {
                                   System.Windows.Forms.MessageBox.Show("1234567890    1234567890");
                                   element.click();
                               }
               */
            }
            _strEmail.Remove(0, _strEmail.Length);
        }

        private void Pass_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender.ToString().Length == 31)
                return;
            string _strPass = sender.ToString().Substring(32);
            _strPass.Remove(0, 1);
            if (_strPass.Length == 0)
                _strPass = "";

            if (httpRequest.Document != null)
            {
                mshtml.HTMLDocument document = (mshtml.HTMLDocument)httpRequest.Document;
                var inputs = document.getElementsByTagName("input");


                int count = 0;
                foreach (mshtml.IHTMLElement element in inputs)
                {
                    if (count == 1)
                    {
                        //element.setAttribute("value", email.Text);
                        //System.Windows.Forms.MessageBox.Show("1234567890");
                        //element.onkeypress();
                        element.setAttribute("value", _strPass.Remove(0, 1));
                        break;
                    }
                    count++;
                }
                
                /*               var btnSubmits = document.getElementsByTagName("button");
                               //document.getElementsByTagName("button")[0].click();
                               foreach (mshtml.IHTMLElement element in btnSubmits)
                               {
                                   System.Windows.Forms.MessageBox.Show("1234567890    1234567890");
                                   element.click();
                               }
               */
            }
            _strPass.Remove(0, _strPass.Length);

            
        }

//         private void AddCart()
//         {
//             
//             //Thread.Sleep(500);
//             if (m_URLValid)
//             {
//                 m_URLValid = false;
//                 return;
//             }
// 
//             if (httpRequest.Document != null)
//             {
//                 mshtml.HTMLDocument document = (mshtml.HTMLDocument)httpRequest.Document;
//                 /******************** Get JSON Datas*********************/
// 
//                 mshtml.IHTMLElementCollection scripts = document.getElementsByTagName("script");
// 
//                 foreach (mshtml.IHTMLElement element in scripts)
//                 {
// 
//                     string strTemp = "eccube.classCategories";
//                     string strElement = element.innerHTML;
//                     //System.Windows.MessageBox.Show(strElement);
//                     
// 
//                     //m_strElement = strElement;
//                     if (strElement == null) continue;
// 
//                     if (strElement.Contains(strTemp))
//                     {
//                         if (!strElement.Contains(";"))
//                             strElement = m_strElement;
//                         m_strJSON = strElement.Substring(30, strElement.IndexOf(';') - 30);
// 
//                         break;
//                     }
//                 }
// 
// 
// //                 string strTemp = "eccube.classCategories";
// //                 string strElementTemp = m_strElement;
// // 
// //                 if (strElementTemp == null) return;    
// // 
// //                 if (strElementTemp.Contains(strTemp))
// //                 {
// //                     m_strJSON = strElementTemp.Substring(26, strElementTemp.IndexOf(';')-26);
// //                 }
//                 
//                 if (m_strJSON == null) {
//                     //System.Windows.MessageBox.Show(m_strJSON);
//                     return;
//                 }
// 
//                 m_StartFlag = false;
//                 m_CheckState = false;
// 
//                 JObject json = JObject.Parse(m_strJSON);
//                 m_strJSON = null;
//                 /***********************************************************/
// 
//                 if (iProduct == jProduct)
//                 {
//                     gif.Visibility = Visibility.Hidden;
//                     btnCart.IsEnabled = true;
//                     m_CartFlag = false;
//                     btnCheckout.IsEnabled = true;
//                     btnDelete.IsEnabled = true;
//                     btnLogout.IsEnabled = true;
//                     //System.Windows.MessageBox.Show("Successfully Carted.");
//                     //                     m_CheckState = false;
//                     //                     m_StartFlag = false;
//                     httpRequest.Navigate("https://www.wtaps.com/cart");
//                     return;
//                 }
//                
//                 var select1 = document.getElementById("classcategory_id1");
//                 var select2 = document.getElementById("classcategory_id2");
// 
//                 mshtml.HTMLSelectElement cbProyectos = select1 as mshtml.HTMLSelectElement;
//                 mshtml.HTMLSelectElement cbProyectos2 = select2 as mshtml.HTMLSelectElement;
//                 //System.Windows.MessageBox.Show(cbProyectos2.length.ToString());
// 
//                 if (cbProyectos == null)
//                 {
// 
//                     //System.Windows.MessageBox.Show("Not Responding...");
//                     
//                     return;
//                 }
//                 //cbProyectos.length == 0 return?
// 
//                 for (var i = 0; i < cbProyectos.length; i++)
//                 {
//                     cbProyectos.selectedIndex = i;
//                     if (cbProyectos.value.Contains("__unselected"))
//                     {
//                         continue;
//                     }
// 
//                     document.getElementById("classcategory_id1").setAttribute("value", cbProyectos.value);
// 
//                     cbProyectos.setAttribute("value", cbProyectos.value);
// 
//                     dynamic colorSelect = cbProyectos as mshtml.DispHTMLSelectElement;
//                     colorSelect.onchange();
//                     m_select1index = Convert.ToString(cbProyectos.value);
// 
//                     //System.Windows.MessageBox.Show(Convert.ToString(cbProyectos.value));
//                     ////////////////////////////////////////////////////////
//                     JToken select2Tokens = json.GetValue(m_select1index);
// 
//                     bool selectFlag = false; 
//                     foreach (JToken selectToken in select2Tokens)
//                     {
// 
//                         foreach (JToken select2_Json in selectToken)
//                         {
//                              
//                             string strIndex_Temp = (string)select2_Json["name"];
//                             string strIndex_value = (string)select2_Json["classcategory_id2"];
// 
//                             m_strSelect2 = strIndex_value;
//                             if (strIndex_Temp != "Please select." && !strIndex_Temp.Contains("(SOLD OUT)"))
//                             {
//                                 if (!strIndex_product_id_temp.Contains((string)select2_Json["product_class_id"]))
//                                 {
//                                     m_strIndex_Temp = strIndex_Temp;
//                                     m_strColor = strColor;
//                                     strIndex_product_id = (string)select2_Json["product_class_id"];
// 
//                                     string strOption = "<option value=" + strIndex_value + ">" + strIndex_Temp + "</option>";
//                                     cbProyectos2.insertAdjacentHTML("afterbegin", strOption);
//                                     
//                                     document.getElementById("classcategory_id2").setAttribute("value", strIndex_value/*cbProyectos2.value*/);
// 
//                                     selectFlag = true;
//                                     //////////////////////////////////////
// //                                     mshtml.IHTMLElement btnAddCart = document.getElementById("add-cart") as mshtml.IHTMLElement;
// // 
// //                                     if (btnAddCart == null)
// //                                         return;
// // 
// //                                     btnAddCart.click();
// // 
// //                                     m_StartFlag = false;
// //                                     m_CartFlagH = true;
//                                     //selectFlag = false;
//                                     //////////////////////////////////////
//                                     strIndex_product_id_temp += "/" + (string)select2_Json["product_class_id"];
// 
//                                     jProduct++;
//                                     break;
//                                 }
//                                 //System.Windows.MessageBox.Show(strIndex_Temp);
// 
//                                 //document.getElementById("classcategory_id2").setAttribute("value", strIndex_value /*cbProyectos2.value*/);
//                                 
//                                 //break;
//                             }
// 
//                         }
//                         //break;
//                     }
// 
//                     if (selectFlag)
//                     {
//                         int count = cbProyectos2.length;
//                         if (count == 1)
//                         {
//                             m_StartFlag = false;
//                             selectFlag = false;
//                             gif.Visibility = Visibility.Hidden;
//                             System.Windows.MessageBox.Show("kkkkk");
//                             //httpRequest.Navigate("");
//                         }
//                         else {
//                             for (var j = 0; j < count; j++)
//                             {
// 
//                                 cbProyectos2.selectedIndex = j;
//                                 //System.Windows.MessageBox.Show(cbProyectos2.value);
//                                 if (cbProyectos2.value == null || cbProyectos2.value == "") continue;
//                                 //cbProyectos2.value = m_strSelect2;
//                                 //System.Windows.MessageBox.Show(cbProyectos2.value);
//                                 document.getElementById("classcategory_id2").setAttribute("value", /*m_strSelect2*/cbProyectos2.value);
// 
//                                 dynamic sizeSelect = cbProyectos2 as mshtml.DispHTMLSelectElement;
//                                 sizeSelect.onchange();
// 
//                                 //m_select2index = strIndex_Temp;
// 
//                                 // button click event!
//                                 mshtml.IHTMLElement btnAddCart = document.getElementById("add-cart") as mshtml.IHTMLElement;
// 
//                                 if (btnAddCart == null)
//                                     return;
// 
//                                 btnAddCart.click();
// 
//                                 break;
//                             }
//                             m_StartFlag = false;
//                             m_CartFlagH = true;
//                             selectFlag = false;
//                             break;
//                         }
//                     }
//                     //break;
//                 }
//             }
//         }

        private void _AddCart()
        {
            if (m_URLValid)
            {
                m_URLValid = false;
                return;
            }

            if (httpRequest.Document != null)
            {
                mshtml.HTMLDocument document = (mshtml.HTMLDocument)httpRequest.Document;

//                 if (iProduct == jProduct)
//                 {
//                     if (iProduct == 0)
//                         return;
// 
//                     gif.Visibility = Visibility.Hidden;
//                     btnCart.IsEnabled = true;
//                     m_CartFlag = false;
//                     btnCheckout.IsEnabled = true;
//                     btnDelete.IsEnabled = true;
//                     btnLogout.IsEnabled = true;
// 
//                     m_CheckState = false;
// //                     m_StartFlag = false;
//                     httpRequest.Navigate("https://www.wtaps.com/cart");
//                     return;
//                 }

                var select1 = document.getElementById("classcategory_id1");
                var select2 = document.getElementById("classcategory_id2");

                mshtml.HTMLSelectElement cbProyectos = select1 as mshtml.HTMLSelectElement;
                mshtml.HTMLSelectElement cbProyectos2 = select2 as mshtml.HTMLSelectElement;
                //System.Windows.MessageBox.Show(cbProyectos2.length.ToString());

                if (cbProyectos == null)
                {

                    //System.Windows.MessageBox.Show("Not Responding...");

                    return;
                }

                

                //cbProyectos.length == 0 return?
                /******************************************************************************/
                //int index =  jProduct;
                for (int index = jProduct; index < listData.Items.Count; index++)
                {
                    m_lvitem = (Product)listData.Items[index];
                    strIndex_product_id = m_lvitem.product_class_id;

                    cbProyectos.setAttribute("value", m_lvitem.select1);

                    string strOption = "<option value=" + m_lvitem.select2 + ">" + m_lvitem.size + "</option>";
                    cbProyectos2.insertAdjacentHTML("afterbegin", strOption);
                    
                    document.getElementById("classcategory_id2").setAttribute("value", m_lvitem.select2);

                   
                    mshtml.IHTMLElement btnAddCart = document.getElementById("add-cart") as mshtml.IHTMLElement;

                    if (btnAddCart == null)
                        return;

                    btnAddCart.click();
                    m_strStatus = "Carting...";

                    //jProduct++;
                    break;
                }
                m_StartFlag = false;
                m_CartFlagH = true;
                               
            }
        }

        private string selectColor2selectSize(string strVal)
        {

            return strVal;
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            httpRequest.Navigate("http://www.wtaps.com/logout");
            m_LogoutFlagH = true;
            btnLogout.IsEnabled = false;

            btnSubmit.IsEnabled = false;
            btnCheckout.IsEnabled = false;
            chk_Time.IsEnabled = true;
            m_Info = false;

            dcolor.Clear();
            dsize.Clear();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //httpRequest.Navigate(urlPath.Text);
            //m_SendFlag = true;
        }

        private void HttpRequest_Navigated_1(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (m_BugFlag)
            {
                m_BugFlag = false;
                var wbMain0 = sender as WebBrowser;
                SetSilent(wbMain0, true);
            }
            

            //SetSilent(httpRequest, true);
            if (m_Flag)
            {
                btnLogin.IsEnabled = true;
                email.IsEnabled = true;
                pass.IsEnabled = true;
//                btnCart.IsEnabled = true;
                //btnProducts.IsEnabled = true;

                gif.Visibility = Visibility.Hidden;

                m_Flag = false;
            }

            if (m_LoginFlag)
            {
                //                btnLogin.Visibility = Visibility.Hidden;
                //                btnLogout.Visibility = Visibility.Visible;
                
//                btnProducts.IsEnabled = true;
 //               btnCart.IsEnabled = true;

                m_LoginValidationFlag = true;

                m_LoginFlag = false;
            }

            if(m_ProductH)
            {
                m_ProductH = false;
                m_ProductT = true;
            }

            if (m_SendFlag)
            {
                //btnCart.IsEnabled = true;
                m_StartFlag = true;
                m_SendFlag = false;
            }

//             if (m_StartFlag)
//             {
//                 //AddCart();
// 
//                 m_StartFlag = false;
//             }

            if (m_CartFlagH)
            {
                m_CartFlagT = true;
                m_CartFlagH = false;
            }

            if (m_CheckoutFlag1)
            {
                //Thread.Sleep(2000);
                m_CheckoutFlag1 = false;
                m_CheckoutFlag2 = true;
            }

            if(m_LogoutFlagH)
            {
                m_LogoutFlagH = false;
                m_LogoutFlagT = true;
            }
            
            if(m_CheckStateH)
            {
                m_CheckState = true;
                m_CheckStateH = false;
            }

            if(m_CCH)
            {
                m_CCH = false;
                m_CCT = true;
            }
            var wbMain = sender as WebBrowser;
            SetSilent(wbMain, true);
        }
        //         [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        //         static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        // 
        //         [DllImport("user32.dll")]
        //         private static extern bool SetForegroundWindow(IntPtr hWnd);
        string[] strtime;
        private void TimerTick(object sender, EventArgs e)
        {
            string strhh = DateTime.Now.Hour.ToString();
            string strmm = DateTime.Now.Minute.ToString();

            
            if(timePicker.Text != null)
            {
                strtime = timePicker.Text.Split(':');
            }



            if (m_TimeCount % m_Second == 0)
            {
 //               if (m_TimeSet)
 //               {
 //                   AutoPlay();
 //               }
                mshtml.HTMLDocument document = (mshtml.HTMLDocument)httpRequest.Document;
                // Put some code here
                //System.Windows.MessageBox.Show("Your email or password is invalid. Try again.");
                if (m_LoginValidationFlag)
                {
                    if(m_TimeSet)
                    {
                        if (strhh.Equals(strtime.GetValue(0)) && strmm.Equals(strtime.GetValue(1)))
                        {
                            m_TimeSet = false;
                            btnLogin.Visibility = Visibility.Hidden;
                            btnLogout.Visibility = Visibility.Visible;
                            btnLogout.IsEnabled = false;
                            //                   //btnLogin.IsEnabled = true;
                            //                   btnCart.IsEnabled = true;
                            btnProducts.IsEnabled = true;
                            gif.Visibility = Visibility.Hidden;

                            m_LoginValidationFlag = false;
                            m_LoginState = true;

                            if (m_AutoProductState)
                            {
                                m_AutoProduct = true;
                                m_AutoProductState = false;
                            }

                        }
                        else if (document.title != "WTAPS / Official Website")
                        {
                            httpRequest.Navigate("https://www.wtaps.com/products/detail/");
                        }
                        else
                        {
                            httpRequest.Navigate("https://www.wtaps.com/products/detail/");
                        }
                    }
                    else if (document.title != "WTAPS / Official Website")
                    {

                        System.Windows.MessageBox.Show("Your email or password is invalid. Try again.");

                        btnLogin.Visibility = Visibility.Visible;
                        //btnLogout.Visibility = Visibility.Hidden;
                        btnLogin.IsEnabled = true;
                        email.Text = "";
                        pass.Text = "";
                        gif.Visibility = Visibility.Hidden;
                        btnProducts.IsEnabled = false;
                        btnCart.IsEnabled = false;

                        m_LoginValidationFlag = false;

                        return;
                    }
                    else if (document.title == "WTAPS / Official Website")
                    {
                        btnLogin.Visibility = Visibility.Hidden;
                        btnLogout.Visibility = Visibility.Visible;
                        //                   //btnLogin.IsEnabled = true;
                        //                   btnCart.IsEnabled = true;
                        btnProducts.IsEnabled = true;
                        gif.Visibility = Visibility.Hidden;

                        m_LoginValidationFlag = false;
                        m_LoginState = true;

                        if (m_AutoProductState)
                        {
                            m_AutoProduct = true;
                            m_AutoProductState = false;
                        }
                    }
/////////////////////////////////////////*/

                }

                if (m_AutoProduct)
                {
                    AutoProduct();
                }

                if (m_BugFlag)
                {
                    m_BugFlag = false;
                    var wbMain = sender as WebBrowser;
                    SetSilent(wbMain, true);
                }

                if (m_ProductT)
                {
                    GetData();
                    //Cart();
                }

                if (m_StartFlag)
                {
                    //                GetData();
                    _AddCart();

                    //return;
                }
                if (m_CartFlagT)
                {
                    //System.Windows.MessageBox.Show("Timer Tick");

                    mshtml.IHTMLElement btnAddCart = document.getElementById("add-cart") as mshtml.IHTMLElement;

                    if (btnAddCart != null)
                    {
                        btnAddCart.click();

                        m_CartPage = true;
                    }
                    //                 else
                    //                     m_CartFlagT = true;

                    //return;
                }



                if (document != null)
                {
                    if (document.title == "WTAPS / 現在のカゴの中")
                    {
                        m_CartFlagT = false;

                        if (m_CartPage)
                        {
                            ChangeData();//////////////////////////////////
                                         //jProduct++;
                            if (iProduct != jProduct)
                            {

                                httpRequest.Navigate(urlPath.Text);

                                m_CartPage = false;

                                m_CheckStateH = true;
                            }
                            else
                            {
                                if (iProduct == 0)
                                {
                                    return;
                                }

                                //                            gif.Visibility = Visibility.Hidden;
                                //                            btnCart.IsEnabled = false;
                                m_CartPage = false;
                                //                            //btnCheckout.IsEnabled = true;
                                btnLogout.IsEnabled = true;


                                //                            gif.Visibility = Visibility.Visible;
                                httpRequest.Navigate("https://www.wtaps.com/cart/buystep");
                                m_CheckoutFlag1 = true;

                                btnCheckout.IsEnabled = false;
                                btnCart.IsEnabled = false;
                                btnLogout.IsEnabled = false;

                                m_CheckState = false;



                            }
                        }

                    }
                    else if (document.title == "WTAPS / 商品購入/確認")
                    {
                        mshtml.IHTMLElementCollection buttons = document.getElementsByTagName("button");

                        foreach (mshtml.IHTMLElement button in buttons)
                        {

                            if(cmbPayMethod.Text == "クレジットカード決済")
                            {
                                mshtml.IHTMLElement radioElement = document.getElementById("shopping_payment_12") as mshtml.IHTMLElement;
                                if (radioElement != null)
                                {
                                    radioElement.click();
                                    //document.getElementById("shopping_payment_12").click();
                                    m_CheckoutFlag1 = true;
                                    //break;

                                }
                            }
                            else
                            {
                                mshtml.IHTMLElement radioElement = document.getElementById("shopping_payment_4") as mshtml.IHTMLElement;
                                if (radioElement != null)
                                {
                                    radioElement.click();
                                    //document.getElementById("shopping_payment_12").click();
                                    m_CheckoutFlag1 = true;
                                    //break;

                                }
                            }
                            

                            //System.Windows.MessageBox.Show(button.innerText);
                            if (button.innerText == "注文する")
                            {
                                //System.Windows.MessageBox.Show("shopping_payment_12 ___ 注文する");
                                button.click();
                                //m_BugFlag = true;
                                //m_CCH = true;
                                break;
                            }
                        }
                    }
                    else if (document.title == "ご購入手続き｜クレジットカード決済")
                    {
                        if (m_CCT)
                        {
                            //System.Windows.MessageBox.Show("ご購入手続き｜クレジットカード決済");
                            //                         btnSubmit.IsEnabled = true;
                            //                         btnLogout.IsEnabled = true;
                            //                         gif.Visibility = Visibility.Hidden;

                            mshtml.IHTMLElementCollection money_spans = document.getElementsByTagName("span");
                            foreach (mshtml.IHTMLElement element in money_spans)
                            {

                                if (element.className == "itemPriceNum")
                                {
                                    if (element.innerHTML != null)
                                    {
                                        lblMoney.Content = element.innerHTML + "円";
                                        lblMoney.Visibility = Visibility.Visible;
                                        gif.Visibility = Visibility.Hidden;
                                        m_Submit = true;
                                        m_CCT = false;

                                        m_strStatus = "Successfully Checked out";
                                        Product tempP;
                                        for (int index = 0; index < listData.Items.Count; index++)
                                        {

                                            lvitem = (Product)listData.Items[index];
                                            lvitem.status = "Successfully Checked out";
                                            tempP = lvitem;

                                            listData.Items.RemoveAt(index);
                                            listData.Items.Insert(index, new Product() { no = index + 1, product = tempP.product, product_class_id = tempP.product_class_id, color = tempP.color, size = tempP.size, status = tempP.status, select1 = tempP.select1, select2 = tempP.select2 });

                                        }
                                        break;
                                    }


                                }
                            }

                        }

                        ///////////////////////////////////
                        ///
                        if (m_Submit)
                        {
                            //mshtml.HTMLDocument document = (mshtml.HTMLDocument)httpRequest.Document;

                            mshtml.IHTMLElementCollection inputs = document.getElementsByTagName("input");

                            foreach (mshtml.IHTMLElement element in inputs)
                            {
                                //System.Windows.MessageBox.Show(element.getAttribute("href"));
                                if (element.getAttribute("name") == "card_number")
                                {
                                    element.setAttribute("value", txtCardNumber.Text);
                                    //element.insertAdjacentText("afterbegin", txtCardNumber.Text);
                                    continue;
                                }

                                if (element.getAttribute("name") == "security_code")
                                {
                                    element.setAttribute("value", txtCode.Text);
                                    //element.innerText = txtCode.Text;
                                    //element.insertAdjacentText("afterbegin", txtCode.Text);
                                    continue;
                                }
                            }

                            mshtml.IHTMLElementCollection selects = document.getElementsByTagName("select");
                            foreach (mshtml.IHTMLElement element in selects)
                            {
                                //System.Windows.MessageBox.Show(element.getAttribute("href"));
                                if (element.getAttribute("name") == "expire_m")
                                {
                                    mshtml.HTMLSelectElement selectM = element as mshtml.HTMLSelectElement;
                                    selectM.setAttribute("value", cmbMonth.Text);

                                    continue;
                                }

                                if (element.getAttribute("name") == "expire_y")
                                {
                                    mshtml.HTMLSelectElement selectY = element as mshtml.HTMLSelectElement;
                                    selectY.setAttribute("value", cmbYear.Text);
                                    continue;
                                }
                            }


                            mshtml.IHTMLElementCollection buttons = document.getElementsByTagName("input");
                            foreach (mshtml.IHTMLElement element in buttons)
                            {
                                if (element.getAttribute("name") == "auth")
                                {
                                    element.click();
                                    m_ErrorFlag = true;
                                    m_Submit = false;
                                    break;
                                }
                            }
                        }

                        //////////////////////////////////////

                        if (m_ErrorFlag)
                        {

                            mshtml.IHTMLElementCollection spans = document.getElementsByTagName("span");
                            foreach (mshtml.IHTMLElement element in spans)
                            {

                                if (element.className == "error_txt")
                                {
                                    if (element.innerHTML != null)
                                    {
                                        btnSubmit.IsEnabled = true;
                                        btnLogout.IsEnabled = true;
                                        gif.Visibility = Visibility.Hidden;
                                        System.Windows.MessageBox.Show(element.innerHTML, "Information------");
                                        m_ErrorFlag = false;
                                        break;
                                    }


                                }
                            }
                        }
                    }
                    else if (document.title == "WTAPS / 商品購入" || document.title == "WTAPS / 商品購入/支払方法選択")
                    {
                        if (!m_CardFlag)
                        {
                            mshtml.IHTMLElementCollection buttons = document.getElementsByTagName("button");

                            foreach (mshtml.IHTMLElement button in buttons)
                            {
                                //System.Windows.MessageBox.Show(button.innerText);
                                if (button.innerText == "注文する")
                                {
                                    Product tempPP;
                                    for (int index = 0; index < listData.Items.Count; index++)
                                    {
                                        m_strStatus = "注文する OK!";
                                        lvitem = (Product)listData.Items[index];
                                        lvitem.status = "注文する OK!";
                                        tempPP = lvitem;

                                        listData.Items.RemoveAt(index);
                                        listData.Items.Insert(index, new Product() { no = index + 1, product = tempPP.product, product_class_id = tempPP.product_class_id, color = tempPP.color, size = tempPP.size, status = tempPP.status, select1 = tempPP.select1, select2 = tempPP.select2 });

                                    }
                                    gif.Visibility = Visibility.Hidden;

                                    //System.Windows.MessageBox.Show("注文する OK!");
                                    button.click();
                                    
                                    //m_BugFlag = true;
                                    //m_CCH = true;
                                    m_CardFlag = true;
                                    
                                    
                                    btnLogout.IsEnabled = true;

                                    m_CheckoutFlag2 = false;
                                    break;

                                }
                            }
                        }



                    }


                }

                //             if (m_CartPage)
                //             {
                //                 m_CartPage = false;
                //                 if (m_CartFlag)
                //                 {
                //                     mshtml.IHTMLElement btnCartStep = document.getElementById("total_box__next_button") as mshtml.IHTMLElement;
                // 
                //                     if (btnCartStep != null)
                //                     {
                //                         if (iProduct != jProduct)
                //                         {
                //                             httpRequest.Navigate(urlPath.Text);
                //                             m_CartPage = false;
                //                             m_CheckState = true;
                //                         }
                // 
                //                     }
                //                 }
                // //                 else
                // //                     m_CartPage = true;
                // 
                //                 //return;
                // 
                //             }

                if (m_CheckState)
                {
                    _AddCart();
                    //m_CartFlagT = false;
                    //m_CartFlag = false ;
                }

                if (m_CheckoutFlag2)
                {

                    LoadingCart();
                }

                if (m_LogoutFlagT)
                {
                    httpRequest.Navigate("https://www.wtaps.com/mypage/login");
                    btnLogin.Visibility = Visibility.Visible;
                    btnLogout.Visibility = Visibility.Hidden;
                    btnLogin.IsEnabled = true;
                    //btnSend.IsEnabled = false;
                    btnCart.IsEnabled = false;
                    email.Text = "";
                    pass.Text = "";
                    btnSubmit.IsEnabled = false;
                    lblMoney.Visibility = Visibility.Hidden;

                    listData.Items.Clear();
                    m_LogoutFlagT = false;
                }

                if (m_Info)
                {
                    //if (m_Count % 5 == 0)
                    //{
                        InformationFile();
                        //m_Count = 0;

                    //}
                    m_Count++;
                }
               
            }
            m_TimeCount++;
        }

        

        Product lvitem;
        private void HttpRequest_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            
        }

        
        public void ChangeData()
        {
            int index;
            int j = 1;
            for (index = 0; index < listData.Items.Count; index++)
            {
                lvitem = (Product)listData.Items[index];
                if (lvitem.product_class_id == strIndex_product_id)
                {
                    m_strStatus = "Successfully Carted!";
                    lvitem.status = "Successfully Carted!";
                    ///*List<Product>*/ items = new List<Product>();
                    //items.Add(new Product() { no = j, product = m_strProduct, product_class_id = strIndex_product_id, color = m_strColor, size = m_strIndex_Temp, status = "Successfully Carted" });
                    listData.Items.RemoveAt(index);
                    listData.Items.Insert(index, new Product() { no = j, product = m_strProduct, product_class_id = strIndex_product_id, color = dcolor[strIndex_product_id], size = dsize[strIndex_product_id], status = "Successfully Carted", select1 = m_select1index, select2 = m_strSelect2 });

                    jProduct++;

                    m_Second = 2;
                    break;
                }
                //items.Add(new Product() { no = j, product = m_strProduct, product_class_id = strIndex_product_id, color = m_strColor, size = m_strIndex_Temp, status = "Carting Failed" });
                j++;

            }

            //m_CartPage = true;
        }

        

        private void gif_MediaEnded(object sender, RoutedEventArgs e)
        {
            gif.Position = new TimeSpan(0, 0, 1);
            gif.Play();
        }
        private void _viewResult(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
//             Customer cust = button.DataContext as Customer;
//             string sDate = cust.Date;
//             string sStatus = cust.Status;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listData.SelectedItems.Count == 0)
            {
                System.Windows.MessageBox.Show("Select a item to delete.");
                return;
            }
                

           
            var selectedProduct = listData.SelectedItems[0] as Product;

            if (selectedProduct == null)
            {
                return;
            }

            /************************************************************/
//             mshtml.HTMLDocument document = (mshtml.HTMLDocument)httpRequest.Document;
// 
//             mshtml.IHTMLElementCollection scripts = document.getElementsByTagName("a");
// 
//             foreach (mshtml.IHTMLElement element in scripts)
//             {
//                 
//                 var strHref = element.getAttribute("href");
//                 System.Windows.MessageBox.Show(Convert.ToString(strHref));
//                 string strProductClassId = selectedProduct.product_class_id;
//                 //System.Windows.MessageBox.Show(element.getAttribute("href"));
//                 if (strHref.Contains(strProductClassId))
//                 {
//                     element.click();
//                                         
//                     break;
//                 }
//             }

            /************************************************************/
            listData.Items.Remove(listData.SelectedItems[0]);

            if (listData.Items.Count == 0)
            {
                btnCheckout.IsEnabled = false;
                
                btnCart.IsEnabled = true;
                return;
            }
        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
//            System.Windows.Forms.Application.Restart();
//            System.Windows.Application.Current.Shutdown();
            if (!m_LoginState)
            {
                System.Windows.MessageBox.Show("You must login first! Try again.", "Information");
                return;
            }

            if (urlPath.Text == "")
            {
                System.Windows.MessageBox.Show("Input URL! Try again.", "Information");
                return;
            }

            if (cmbPayMethod.Text == "クレジットカード決済")
            {
                if (txtCardNumber.Text == "")
                {
                    System.Windows.MessageBox.Show("Please input card number.", "Information");
                    return;
                }

                if (cmbMonth.Text == "")
                {
                    System.Windows.MessageBox.Show("Please input month.", "Information");
                    return;
                }
                if (cmbYear.Text == "")
                {
                    System.Windows.MessageBox.Show("Please input year.", "Information");
                    return;
                }
                if (txtCode.Text == "")
                {
                    System.Windows.MessageBox.Show("Please input security code.", "Information");
                    return;
                }
            }

            /*            if (cmbColor.Text == "")
                        {
                            System.Windows.MessageBox.Show("Please input color.", "Information");
                            return;
                        }
                        if (cmbSize.Text == "")
                        {
                            System.Windows.MessageBox.Show("Please input size.", "Information");
                            return;
                        }
            */

            httpRequest.Navigate(urlPath.Text);

            gif.Visibility = Visibility.Visible;

            iProduct = 0;
            jProduct = 0;

            m_ProductH = true;

            btnProducts.IsEnabled = false;
            btnCart.IsEnabled = false;

            btnLogout.IsEnabled = false;
            m_strStatus = "Waiting for schedule";

            m_Info = true;
        }

        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            iProduct = listData.Items.Count;

            btnCart.IsEnabled = false;
            btnProducts.IsEnabled = false;
            btnDelete.Visibility = Visibility.Hidden;
            gif.Visibility = Visibility.Visible;

            //m_SendFlag = true;
            m_StartFlag = true;
        }

        private void BtnCheckout_Click(object sender, RoutedEventArgs e)
        {
            gif.Visibility = Visibility.Visible;
            m_BugFlag = true;
            httpRequest.Navigate("https://www.wtaps.com/cart/buystep");

            m_CheckoutFlag1 = true;

            btnCheckout.IsEnabled = false;
            btnCart.IsEnabled = false;
            btnLogout.IsEnabled = false;        

        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if(cmbPayMethod.Text == "クレジットカード決済")
            {
                if (txtCardNumber.Text == "")
                {
                    System.Windows.MessageBox.Show("Please input card number.", "Information");
                    return;
                }

                if (cmbMonth.Text == "")
                {
                    System.Windows.MessageBox.Show("Please input month.", "Information");
                    return;
                }
                if (cmbYear.Text == "")
                {
                    System.Windows.MessageBox.Show("Please input year.", "Information");
                    return;
                }
                if (txtCode.Text == "")
                {
                    System.Windows.MessageBox.Show("Please input security code.", "Information");
                    return;
                }
            }

            


            // txtCardNumber.Text; 
            if (httpRequest.Document != null)
            {
                mshtml.HTMLDocument document = (mshtml.HTMLDocument)httpRequest.Document;

                mshtml.IHTMLElementCollection inputs = document.getElementsByTagName("input");

                foreach (mshtml.IHTMLElement element in inputs)
                {
                    //System.Windows.MessageBox.Show(element.getAttribute("href"));
                    if (element.getAttribute("name") == "card_number")
                    {
                        element.setAttribute("value", txtCardNumber.Text);
                        //element.insertAdjacentText("afterbegin", txtCardNumber.Text);
                        continue;
                    }

                    if(element.getAttribute("name") == "security_code")
                    {
                        element.setAttribute("value", txtCode.Text);
                        //element.innerText = txtCode.Text;
                        //element.insertAdjacentText("afterbegin", txtCode.Text);
                        continue;
                    }
                }

                mshtml.IHTMLElementCollection selects = document.getElementsByTagName("select");
                foreach (mshtml.IHTMLElement element in selects)
                {
                    //System.Windows.MessageBox.Show(element.getAttribute("href"));
                    if (element.getAttribute("name") == "expire_m")
                    {
                        mshtml.HTMLSelectElement selectM = element as mshtml.HTMLSelectElement;
                        selectM.setAttribute("value", cmbMonth.Text);

                        continue;
                    }

                    if (element.getAttribute("name") == "expire_y")
                    {
                        mshtml.HTMLSelectElement selectY = element as mshtml.HTMLSelectElement;
                        selectY.setAttribute("value", cmbYear.Text);
                        continue;
                    }
                }
                

                mshtml.IHTMLElementCollection buttons = document.getElementsByTagName("input");
                foreach (mshtml.IHTMLElement element in buttons)
                {
                    if (element.getAttribute("name") == "auth")
                    {
                        element.click();
                        m_ErrorFlag = true;
                        break;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
                    }
                }
            }
        }

        private void BtnTimeSet_Click(object sender, RoutedEventArgs e)
        {
            if(timePicker.Text == null)
            {
                System.Windows.MessageBox.Show("Select time.", "Information");
                
                timePicker.Focus();
                return;
            }

            if (email.Text == "" || !email.Text.Contains("@") || !email.Text.Contains("."))
            {
                System.Windows.MessageBox.Show("Invalid Email. Try again.", "Information");
                email.Text = "";
                email.Focus();
                return;
            }

            if (pass.Text == "")
            {
                System.Windows.MessageBox.Show("Input Password. Try again.", "Information");
                pass.Focus();
                return;
            }
            
            ////////////////////////////////////////////////////////////////////////////////

            if (urlPath.Text == "")
            {
                System.Windows.MessageBox.Show("Input URL! Try again.", "Information");
                return;
            }

            if(cmbPayMethod.Text == "クレジットカード決済")
            {
                if (txtCardNumber.Text == "")
                {
                    System.Windows.MessageBox.Show("Please input card number.", "Information");
                    return;
                }

                if (cmbMonth.Text == "")
                {
                    System.Windows.MessageBox.Show("Please input month.", "Information");
                    return;
                }
                if (cmbYear.Text == "")
                {
                    System.Windows.MessageBox.Show("Please input year.", "Information");
                    return;
                }
                if (txtCode.Text == "")
                {
                    System.Windows.MessageBox.Show("Please input security code.", "Information");
                    return;
                }
            }

            

/*            if (cmbColor.Text == "")
            {
                System.Windows.MessageBox.Show("Please input color.", "Information");
                return;
            }
            if (cmbSize.Text == "")
            {
                System.Windows.MessageBox.Show("Please input size.", "Information");
                return;
            }
*/




            m_TimeSet = true;
            m_AutoProductState = true;
            btnTimeSet.IsEnabled = false;
            timePicker.IsEnabled = false;

            AutoPlay();
        }

        private void AutoPlay()
        {
            DateTime.Now.ToString();
            string strhh = DateTime.Now.Hour.ToString();
            string strmm = DateTime.Now.Minute.ToString();

            string[] strtime = timePicker.Text.Split(':');

//            if (strhh.Equals(strtime.GetValue(0)) && strmm.Equals(strtime.GetValue(1)))
//            {
                //System.Windows.MessageBox.Show("aaaaaaa");
//                m_TimeSet = false;

                /********Login************/
                mshtml.HTMLDocument document = (mshtml.HTMLDocument)httpRequest.Document;
                var btnSubmits = document.getElementsByTagName("button");
                //document.getElementsByTagName("button")[0].click();
                //var dd = document.getElementById("login_pass").getAttribute("value", 0);
                //
                foreach (mshtml.IHTMLElement element in btnSubmits)
                {
                    //System.Windows.Forms.MessageBox.Show("1234567890    1234567890");
                    element.click();
                }
                ///////////////////////////////////////////////////
                ////////////////////////////////////////////////////
                gif.Visibility = Visibility.Visible;
                btnLogin.IsEnabled = false;
                m_LoginFlag = true;

                btnProducts.IsEnabled = false;

                chk_Time.IsEnabled = false;

            m_strStatus = "Waiting for schedule";
            m_Info = true;

            /*****************************************/

            /**************Product***************/
            //               httpRequest.Navigate(urlPath.Text);

            //                gif.Visibility = Visibility.Visible;
            //
            //                iProduct = 0;
            //                jProduct = 0;

            //                m_ProductH = true;

            //                btnProducts.IsEnabled = false;
            //                btnCart.IsEnabled = false;

            //                btnLogout.IsEnabled = false;
            //            }
        }

        private void AutoProduct()
        {
            m_AutoProduct = false;
            httpRequest.Navigate(urlPath.Text);
            
            gif.Visibility = Visibility.Visible;
            iProduct = 0;
            jProduct = 0;

            m_ProductH = true;

            btnProducts.IsEnabled = false;
            btnCart.IsEnabled = false;

            btnLogout.IsEnabled = false;

         //   m_strStatus = "Waiting for schedule";
         //   m_Info = true;
        }
        private void Chk_Time_Click(object sender, RoutedEventArgs e)
        {
            bool checkState = (bool)chk_Time.IsChecked;
            if (checkState)
            {
                btnTimeSet.IsEnabled = true;
                timePicker.IsEnabled = true;

                btnLogin.IsEnabled = false;
                btnProducts.IsEnabled = false;
            }
            else {
                btnTimeSet.IsEnabled = false;
                timePicker.IsEnabled = false;

                btnLogin.IsEnabled = true;
                //btnProducts.IsEnabled = true;
            }
        }

        private void CmbPayMethod_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //System.Windows.MessageBox.Show(sender.ToString());
            if (cmbPayMethod.Text == "クレジットカード決済" )
            {
                showCardInfo(false);
            }
            else
            {
                showCardInfo(true);
            }

            if (cmbPayMethod.Text == "")
                showCardInfo(true);
        }

        private void showCardInfo(bool payFlag)
        {
            if(payFlag)
            {
                txtCardNumber.IsEnabled = true;
                cmbMonth.IsEnabled = true;
                cmbYear.IsEnabled = true;
                txtCode.IsEnabled = true;

            }
            else
            {
                txtCardNumber.IsEnabled = false;
                cmbMonth.IsEnabled = false;
                cmbYear.IsEnabled = false;
                txtCode.IsEnabled = false;
            }
        }

        private void LoadingCart()
        {
            Product tempP;
            for (int index = 0; index < listData.Items.Count; index++)
            {
                m_strStatus = "Checking out...";
                lvitem = (Product)listData.Items[index];
                lvitem.status = "Checking out . . .";
                tempP = lvitem;

                listData.Items.RemoveAt(index);
                listData.Items.Insert(index, new Product() { no = index + 1, product = tempP.product, product_class_id = tempP.product_class_id, color = tempP.color, size = tempP.size, status = tempP.status, select1 = tempP.select1, select2 = tempP.select2 });

            }
            mshtml.HTMLDocument document = (mshtml.HTMLDocument)httpRequest.Document;
            if (document.title == "WTAPS / 商品購入" || document.title == "WTAPS / 商品購入/支払方法選択")
            {

                if (cmbPayMethod.Text == "クレジットカード決済")
                {
                    mshtml.IHTMLElementCollection buttons = document.getElementsByTagName("button");

                    foreach (mshtml.IHTMLElement button in buttons)
                    {

                        //System.Windows.MessageBox.Show(button.innerText);
                        if (button.innerText == "次へ")
                        {
                            m_CheckoutFlag2 = false;
                            button.click();
                            m_BugFlag = true;
                            m_CCH = true;
                            break;

                        }
                    }
                }
                else
                {
                    if (!m_CardFlag)
                    {
                        mshtml.IHTMLElementCollection buttons = document.getElementsByTagName("button");

                        foreach (mshtml.IHTMLElement button in buttons)
                        {
                            if (m_CheckCard)
                            {

                                mshtml.IHTMLElementCollection labels = document.getElementsByTagName("label");
                                foreach (mshtml.IHTMLElement label in labels)
                                {
                                    if(label.innerText.Contains("代金引換"))
                                    {
                                        m_CheckCard = false;
                                        label.click();
                                    }
                                }

                                if (m_CheckCard && button.innerText == "次へ")
                                {
                                    m_CheckoutFlag2 = false;
                                    button.click();
                                    m_BugFlag = true;
                                    //m_CCH = true;
                                    break;

                                }
                                break;

                                /*                                mshtml.IHTMLElement radioElement = document.getElementById("shopping_payment_4") as mshtml.IHTMLElement;
                                                                if (radioElement != null)
                                                                {
                                                                    m_CheckCard = false;
                                                                    //document.getElementById("shopping_payment_4").click();
                                                                    radioElement.click();
                                                                    //break;

                                                                }
                                                               else
                                                                {

                                                                    if (button.innerText == "次へ")
                                                                    {
                                                                        m_CheckoutFlag2 = false;
                                                                        button.click();
                                                                        m_BugFlag = true;
                                                                        //m_CCH = true;
                                                                        break;

                                                                    }
                                                                }
                                */
                            }

                            if (!m_CheckCard && button.innerText == "次へ")
                            {
                                m_CheckoutFlag2 = false;
                                button.click();
                                m_BugFlag = true;
                                //m_CCH = true;
                                break;

                            }
                            //System.Windows.MessageBox.Show(button.innerText);
              /*              if (button.innerText == "注文する")
                            {
                                //button.click();
                                //m_BugFlag = true;
                                //m_CCH = true;
                                m_CardFlag = true;
                                Product tempPP;
                                for (int index = 0; index < listData.Items.Count; index++)
                                {
                                    m_strStatus = "注文する OK!";
                                    lvitem = (Product)listData.Items[index];
                                    lvitem.status = "注文する OK!";
                                    tempPP = lvitem;

                                    listData.Items.RemoveAt(index);
                                    listData.Items.Insert(index, new Product() { no = index + 1, product = tempPP.product, product_class_id = tempPP.product_class_id, color = tempPP.color, size = tempPP.size, status = tempPP.status, select1 = tempPP.select1, select2 = tempPP.select2 });

                                }
                                gif.Visibility = Visibility.Hidden;
                                btnLogout.IsEnabled = true;

                                m_CheckoutFlag2 = false;
                                break;

                            }*/
                        }
                    }
                    
                }
                
            }
            
        }
        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 1 && i <= 9999;
        }

        public void Cart()
        {
            iProduct = listData.Items.Count;

            btnCart.IsEnabled = false;
            btnProducts.IsEnabled = false;
            btnDelete.Visibility = Visibility.Hidden;
            gif.Visibility = Visibility.Visible;

            //m_SendFlag = true;
            m_StartFlag = true;
        }

        private  void SetSilent(WebBrowser browser, bool silent)
        {
            //if (browser == null)
            //    throw new ArgumentNullException("httpRequest");

            // get an IWebBrowser2 from the document
            //IOleServiceProvider sp = browser.Document as IOleServiceProvider;
            
            IOleServiceProvider sp = httpRequest.Document as IOleServiceProvider;
            if (sp != null)
            {
                Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

                object webBrowser;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);
                if (webBrowser != null)
                {
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
                }
            }
        }
        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IOleServiceProvider
        {
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
        }


    }


    public class Product
    {
        public int no { get; set; }
        public string product { get; set; }
        public string product_class_id { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public string status { get; set; }
        public string select1 { get; set; }
        public string select2 { get; set; }

    }
}
