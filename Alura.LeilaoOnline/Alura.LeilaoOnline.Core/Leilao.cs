using System.Collections.Generic;
using System.Linq;

namespace Alura.LeilaoOnline.Core
{
    public enum EstadoLeilao
    {
        LeilaoAntesdoPregao,
        LeilaoEmAndamento, 
        LeilaoFinalizado
    }
    public class Leilao
    {
        private Interessada _ultimoCliente;
        private IModalidadeAvaliacao _avaliador; 
        private IList<Lance> _lances;
        public IEnumerable<Lance> Lances => _lances;
        public string Peca { get; }
        public Lance Ganhador { get; set; }
        public EstadoLeilao Estado {get; private set; }

        public Leilao(string peca, IModalidadeAvaliacao avaliador )
        {
            Peca = peca;
            _avaliador = avaliador;
            _lances = new List<Lance>();
            Estado = EstadoLeilao.LeilaoAntesdoPregao; 
        }

        private bool NovoLanceEhAceito(Interessada cliente) => (Estado == EstadoLeilao.LeilaoEmAndamento && _ultimoCliente != cliente);

        public void RecebeLance(Interessada cliente, double valor)
        {
            if(NovoLanceEhAceito(cliente))
            {
                _lances.Add(new Lance(cliente, valor));
                _ultimoCliente = cliente;
            }
        }

        public void IniciaPregao()
        {
            Estado = EstadoLeilao.LeilaoEmAndamento;
        }

        public void TerminaPregao()
        {
            if (Estado != EstadoLeilao.LeilaoEmAndamento)
            {
                throw new System.InvalidOperationException("Não é possível terminar o pregão sem que ele tenha começado. Para isso, utilize o método IniciaPregao().");
            }

            Ganhador = _avaliador.Avalia(this);

            Estado = EstadoLeilao.LeilaoFinalizado;
        }
    }
}