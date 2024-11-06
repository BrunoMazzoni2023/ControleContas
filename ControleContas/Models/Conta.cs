namespace ControleContas.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public required string Descricao { get; set; } // Campo obrigatório
        public decimal Valor { get; set; }

        // Inicializa DataCadastro com a data atual
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime Vencimento { get; set; }

        public required string Parcela { get; set; } // Se for usado para representar número/ordem de parcelas, isso está correto

        public StatusConta StatusConta { get; set; } // Enum para status da conta
    }

    public enum StatusConta
    {
        Pago,
        Pendente,
        Vencido
    }
}
