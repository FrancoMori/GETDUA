using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using NUnit.Framework;
using System.Threading;
using System.Data;
using System.IO;


namespace UIDUA
{
    class UISunat
    {
        IWebDriver browser;
        [SetUp]
        public void Startbrowser()
        {
            browser = new ChromeDriver(@"C:\temp\");
        }
        [Test]
        public void Test()
        {
            browser.Url = @"http://www.aduanet.gob.pe/cl-ad-consdespade/ConsExportIAServlet?pTipoConsulta=aduana";
        }
        [TearDown]

        public void Closebrowser()
        {
            Adaptador obj = new Adaptador();
            var ructenofil = "20100103223";
            var fechainicial = browser.FindElement(By.Name("FecInicial"));
            fechainicial.SendKeys("01/12/2019");
            var fechafinal = browser.FindElement(By.Name("FecFinal"));
            fechafinal.SendKeys("31/12/2019");

            var aduana = browser.FindElement(By.Name("dato"));
            aduana.SendKeys("118");

            var clasificado = browser.FindElement(By.Name("agrupadoPor"));
            clasificado.SendKeys("EXPORTADOR");

            var ruc = browser.FindElement(By.Name("codagrupadoPor"));
            ruc.SendKeys(ructenofil);

            var enviar = browser.FindElement(By.ClassName("form-button"));
            enviar.Click();

            var link = browser.FindElements(By.XPath(".//a"));
            var cad = "";
            foreach (var i in link)
            {
                if (i.Text.Contains(ructenofil))
                {
                    i.Click();
                }
                //cad = cad + i.Text + ",";
            }
            var masterpages = browser.FindElements(By.XPath(".//a")); // extraer la cantidad de paginación que es encuentra en la tabla
            var contador_masterpages = 0;
            foreach (var i in masterpages)
            {
                if (i.Text.Contains("|"))
                {
                    contador_masterpages += 1;
                }
            }


            
            var hostdt = new DataTable("host"); //datatable que almacena datos
            try
            {
                // Crea la carpeta DUA en caso no exista
                if (!Directory.Exists(@"C:\temp\DUA")) {
                    Directory.CreateDirectory(@"C:\temp\DUA");
                }
                var tablehtml = browser.FindElement(By.ClassName("beta"));

                // descarga de html por paginación
                File.WriteAllText(@"C:\temp\DUA\Fpage.html", tablehtml.GetAttribute("outerHTML"));

                var nextpages = browser.FindElement(By.LinkText("Siguiente"));
                nextpages.Click();  //Acceso a la primera hoja
                tablehtml = browser.FindElement(By.ClassName("beta"));

                // descarga de html por paginación
                File.WriteAllText(@"C:\temp\DUA\Spage.html", tablehtml.GetAttribute("outerHTML"));

                for (var n = 0; n < contador_masterpages - 1; n++)
                {
                    nextpages = browser.FindElement(By.LinkText("Siguiente"));
                    nextpages.Click(); //navegación de primera página a última
                    tablehtml = browser.FindElement(By.ClassName("beta"));
                    File.WriteAllText(@"C:\temp\DUA\Spage" + n + ".html", tablehtml.GetAttribute("outerHTML"));
                }
            }
            catch (Exception ex)
            {
                //Ingresa excepción sin no encuentra paginación
                //buscar en el file plano
                var table_html = browser.FindElement(By.ClassName("beta"));
                DataTable temp = obj.ConvertHTMLTablesToDataTable(table_html.GetAttribute("outerHTML"));
                //descarga de html total sin paginación
                temp.WriteXml(@"C:\temp\DUA\TPage.html", XmlWriteMode.WriteSchema);
            }
            finally
            {
                browser.Close();
               /*
                * 
                Ejecución de sentencias bajo lectura de File HTML
                usar HTMLDocuemnt en caso de manipular archivos DOM.

                 */
            }
        }
    }
}
