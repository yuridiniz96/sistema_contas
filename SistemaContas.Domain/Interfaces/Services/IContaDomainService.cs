using SistemaContas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SistemaContas.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface para definir os métodos da classe ContaDomainService
    /// </summary>
    public interface IContaDomainService
    {
        #region Métodos abstratos
        void CadastrarConta(Conta conta);
        void AtualizarConta(Conta conta);
        void ExcluirConta(Guid idConta);
        List<Conta> ConsultarContas(DateTime dataMin, DateTime dataMax);
        Conta ObterConta(Guid idConta);


        byte[] GerarRelatorioExcel(List<Conta> contas);
        byte[] GerarRelatorioPdf(List<Conta> contas);
        #endregion
    }
}