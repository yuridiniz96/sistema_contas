using Bogus;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;
namespace SistemaContas.Tests
{
    public class ContasCadastroTest
    {
        [Fact]
        public void CadastrarContaComSucesso()
        {
            //abrir o navegador
            var driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            //acessando a página que sera testada
            driver.Navigate().GoToUrl

            ("http://localhost:5291/Contas/Cadastro");

            var faker = new Faker("pt_BR");
            var nomeConta = faker.Commerce.Product();

            //preenchendo os campos do formulário
            var nome = driver.FindElement(By.XPath("//*[@id=\"Nome\"]"));
            nome.Clear();
            nome.SendKeys(nomeConta);

            var data = driver.FindElement(By.XPath("//*[@id=\"Data\"]"));
            data.Clear();
            data.SendKeys(DateTime.Now.ToString("dd/MM/yyyy"));

            var valor = driver.FindElement(By.XPath("//*[@id=\"Valor\"]"));
            valor.Clear();
            valor.SendKeys(faker.Commerce.Price(2));

            var tipo = driver.FindElement(By.XPath("//*[@id=\"Tipo\"]"));
            tipo.Click();
            //clicar no botao de confirmaçao
            var botao = driver.FindElement

            (By.XPath("/html/body/div/form/div[3]/div/input"));

            botao.Click();
            //capturando a mensagem exibida pelo sistema
            var mensagem = driver.FindElement
            (By.XPath("/html/body/div[1]"));

            //comparando o resultado esperado com o resultado obtido
            var resultadoEsperado = $"Sucesso! Conta '{nomeConta}',cadastrada com sucesso!";

            var resultadoObtido = mensagem.Text;
            Assert.Equal(resultadoEsperado, resultadoObtido);

            //fechando o navegador
            driver.Close();
            driver.Quit();
        }
    }
}