﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SistemaContas.Domain.Entities
{
    /// <summary>
    /// Classe de entidade de domínio
    /// </summary>
    public class Conta
    {
        public Guid IdConta { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public TipoConta Tipo { get; set; }
    }
}