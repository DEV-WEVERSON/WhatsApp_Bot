using System;
using System.Threading.Tasks;
using System.Xml.XPath;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WhatsAppBot.Controllers
{
    public class BotController : Controller
    {
        private static readonly IWebDriver _driver = new ChromeDriver();

        private const string Contatos = "//*[@id=\"main\"]/header/div[2]/div[2]/span";
        private const string BtnEnviar = "/html/body/div[1]/div/div/div[4]/div/footer/div[1]/div[3]/button";
        private const string MensagemBox = "//*[@id=\"main\"]/footer/div[1]/div[2]/div/div[2]";
        private const string PrimeiroContato = "//*[@id=\"pane-side\"]/div[1]/div/div/div[1]/div";
        private const string NovoChat = "//*[@id=\"side\"]/div[1]/div/label/div/div[2]";
        private const string MensagemNaoLida = "//*[@id=\"pane-side\"]/div[1]/div/div/div[15]/div/div/div[2]/div[2]/div[2]/span[1]/div[2]/span";
        private const string ListaConversas = "//*[@id=\"pane-side\"]/div[1]/div/div";
        private const string Conversa = "//*[@id=\"pane-side\"]/div[1]/div/div/div[2]";
        private const string NomeContato = "//*[@id=\"pane-side\"]/div[1]/div/div/div[11]/div/div/div[2]/div[1]/div[1]/span";

       
        // GET
        public IActionResult IniciarBot()
        {
            
            _driver.Navigate().GoToUrl("https://web.whatsapp.com");
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
        
        public void EnviarMensagem(string mensagem)
        {
            EscreverMensagem(mensagem);
            Click();
        }

        public async void ProcurarContato(string nomeContato)
        {
            var novoChat = _driver.FindElement(By.XPath(NovoChat));
            novoChat.Clear();
            novoChat.Click();
            novoChat.SendKeys(nomeContato);
            
            await Task.Delay(0700);

            try
            {
                var contatoProcurado = _driver.FindElement(By.XPath(PrimeiroContato));
                contatoProcurado.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine("Não encontrado");
            }
            
        }

        public void EscutarConversas()
        {
            var listaConversas = _driver.FindElement(By.XPath(ListaConversas));

            var conversas = listaConversas.FindElements(By.XPath(Conversa));

            foreach (var conversa in conversas)
            {
                if (conversa.FindElement(By.XPath(MensagemNaoLida)) != null)
                {
                    var nomeContato = conversa.FindElement(By.XPath(NomeContato)).Text;
                    ProcurarContato(nomeContato);
                    EnviarMensagem("Olá. Mensagem de teste");
                }
                
            }
            
        }
    }
}