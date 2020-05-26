using System;
using System.Xml.XPath;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WhatsAppBot.Controllers
{
    public class BotController : Controller
    {
        private readonly IWebDriver _driver = new ChromeDriver();

        private const string Contatos = "'//*[@id=\"main\"]/header/div[2]/div[2]/span'";
        private const string BtnEnviar = "/html/body/div[1]/div/div/div[4]/div/footer/div[1]/div[3]/button";
        private const string ProcurarContato = "'//*[@id=\"app\"]/div/div/div[2]/div[1]/span/div/span/div/div[1]/div/label/div/div[2]'";
        private const string MensagemBox = "//*[@id=\"main\"]/footer/div[1]/div[2]/div/div[2]";
        private const string PrimeiroContato = "'//*[@id=\"app\"]/div/div/div[2]/div[1]/span/div/span/div/div[2]/div/div/div/div[2]/div'";
        private const string NovoChat = "'//*[@id=\"side\"]/header/div[2]/div/span/div[2]/div'";
        
        
        // GET
        public IActionResult IniciarBot()
        {
            
            _driver.Navigate().GoToUrl("https://web.whatsapp.com");
            
            EnviarMensagem("teste 123");
            return View("Index");
        }

        private IWebElement GetElement(string xpath, int attempts = 5, int cont = 0)
        {
            try
            {
                var element = _driver.FindElement(By.XPath(xpath));
                return element;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                if (cont < attempts)
                {
                    GetElement(xpath, attempts, ++cont);
                }
                else
                {
                    Console.WriteLine("Não encontrado");
                }
                return null;
            }
        }

        private void Click()
        {
            var botao = GetElement(BtnEnviar);
            botao.Click();
        }

        private void EscreverMensagem(string mensagem)
        {
            var inputMensagem = GetElement(MensagemBox);
            inputMensagem.SendKeys(mensagem);
        }
        [HttpPost]
        public void EnviarMensagem(string mensagem)
        {
            EscreverMensagem(mensagem);
            Click();
        }
    }
}