using Microsoft.AspNetCore.Mvc;
using SistemaContas.Domain.Entities;
using SistemaContas.Domain.Interfaces.Services;
using SistemaContas.Mvc.Models;
namespace SistemaContas.Mvc.Controllers
{
    public class ContasController : Controller
    {
        //atributo
        private readonly IContaDomainService _contaDomainService;
        //método construtor para inicializar
        //os atributos da classe (injeção de dependência)
        public ContasController(IContaDomainService contaDomainService)
        {
            _contaDomainService = contaDomainService;
        }
        //ROTA: /Contas/Dashboard
        public IActionResult Dashboard()
        {
            var model = new ContasDashboardModel();
            try
            {
                model.DataInicio = new DateTime (DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                model.DataFim = new DateTime (DateTime.Now.Year, DateTime.Now.Month,DateTime.DaysInMonth (DateTime.Now.Year, DateTime.Now.Month)).ToString("yyyy-MM-dd");
                
                //consultando as contas:
                var contas = _contaDomainService.ConsultarContas (DateTime.Parse(model.DataInicio), DateTime.Parse(model.DataFim));
                
                //gerando os dados para o gráfico
                TempData["TotalContasReceber"] = contas.Where (c => c.Tipo == TipoConta.Receber).Sum(c => c.Valor);

                TempData["TotalContasPagar"] = contas.Where (c => c.Tipo == TipoConta.Pagar).Sum(c => c.Valor);

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }
            return View(model);
        }


        //ROTA: POST /Contas/Dashboard
        [HttpPost]
        public IActionResult Dashboard(ContasDashboardModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //consultando as contas:
                    var contas = _contaDomainService.ConsultarContas(DateTime.Parse(model.DataInicio), DateTime.Parse(model.DataFim));
                    
                    //gerando os dados para o gráfico
                    TempData["TotalContasReceber"] = contas.Where (c => c.Tipo == TipoConta.Receber).Sum(c => c.Valor);

                    TempData["TotalContasPagar"] = contas.Where (c => c.Tipo == TipoConta.Pagar).Sum(c => c.Valor);
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            return View(model);
        }


        //ROTA: /Contas/Cadastro
        public IActionResult Cadastro()
        {
            return View();
        }
        [HttpPost] //SUBMIT POST /Contas/Cadastro
        public IActionResult Cadastro(ContasCadastroModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var conta = new Conta();
                    conta.IdConta = Guid.NewGuid();
                    conta.Nome = model.Nome;
                    conta.Data = DateTime.Parse(model.Data);
                    conta.Valor = decimal.Parse(model.Valor);
                    if (model.Tipo.Equals("Receber"))
                        conta.Tipo = TipoConta.Receber;
                    else if (model.Tipo.Equals("Pagar"))
                        conta.Tipo = TipoConta.Pagar;
                    _contaDomainService.CadastrarConta(conta);
                    TempData["MensagemSucesso"] = $"Conta '{conta.Nome}', cadastrada com sucesso!";

                    ModelState.Clear();
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            return View();
        }
        //ROTA: /Contas/Consulta
        public IActionResult Consulta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Consulta (ContasConsultaModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dataMin = DateTime.Parse(model.DataInicio);
                    var dataMax = DateTime.Parse(model.DataFim);
                    var contas = _contaDomainService.ConsultarContas(dataMin, dataMax);
                    var nomeArquivo = $"contas_{ DateTime.Now.ToString("ddMMyyyyHHmmss")}";
                    var tipoArquivo = string.Empty;
                    byte[] arquivo = null;

                    switch (model.Formato)
                    {
                        case "Excel":
                            nomeArquivo += ".xlsx";
                            tipoArquivo = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            arquivo = _contaDomainService.GerarRelatorioExcel(contas);

                            //download do relatório

                            return File(arquivo, tipoArquivo, nomeArquivo);

                        case "Pdf":
                            nomeArquivo += ".pdf";
                            tipoArquivo = "application/pdf";
                            arquivo = _contaDomainService.GerarRelatorioPdf(contas);
                            //download do relatório
                            return File(arquivo, tipoArquivo, nomeArquivo);

                        case "Html":
                            model.Contas = contas;
                            return View(model);

                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            return View();
        }

        //ROTA: /Contas/Exclusao
        public IActionResult Exclusao(Guid id)
        {
            try
            {
                _contaDomainService.ExcluirConta(id);
                TempData["MensagemSucesso"] = "Conta excluída com sucesso!";
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }
            //redirecionar de volta para a página de consulta
            return RedirectToAction("Consulta");
        }

        //ROTA: /Contas/Edicao/{id}
        public IActionResult Edicao(Guid id)
        {
            var model = new ContasEdicaoModel();
            try
            {
                //buscar a conta atraves do id
                var conta = _contaDomainService.ObterConta(id);
                //preenchendo os campos da model
                model.IdConta = conta.IdConta;
                model.Nome = conta.Nome;
                model.Data = conta.Data.ToString("yyyy-MM-dd");
                model.Valor = conta.Valor.ToString();
                model.Tipo = conta.Tipo.ToString();
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }
            return View(model);
        }

        //ROTA: POST /Contas/Edicao
        [HttpPost]
        public IActionResult Edicao(ContasEdicaoModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var conta = new Conta();
                    conta.IdConta = model.IdConta;
                    conta.Nome = model.Nome;
                    conta.Data = Convert.ToDateTime(model.Data);
                    conta.Valor = decimal.Parse(model.Valor);

                    if (model.Tipo.Equals("Receber"))
                        conta.Tipo = TipoConta.Receber;
                    else if (model.Tipo.Equals("Pagar"))
                        conta.Tipo = TipoConta.Pagar;   

                    _contaDomainService.AtualizarConta(conta);

                    TempData["MensagemSucesso"] = $"Conta '{conta.Nome}', atualizada com sucesso!";

                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            return View(model);
        }
    }
}