using Xunit;
using Alura.LeilaoOnline.Core;
using System.Linq;

namespace Alura.LeilaoOnline.Tests
{
    public class LeilaoRecebeLance
    {
        [Theory]
        [InlineData(4, new double[] { 1000, 900, 1000, 990 })]
        [InlineData(2, new double[] { 800, 900 })]
        public void NaoPermiteNovosLancesAoLeilaoSerFinalizado(int qtdEsperada, double[] ofertas)
        {
            // Arranje - cenário
            var avaliador = new MaiorValor();
            var leilao = new Leilao("Van Gogh", avaliador);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);
            leilao.IniciaPregao();

            for (int i = 0; i < ofertas.Length; i++)
            {
                var valor = ofertas[i];
                if ((i % 2) == 0)
                {
                    leilao.RecebeLance(fulano, valor);
                }
                else
                {
                    leilao.RecebeLance(maria, valor);
                }
            }

            leilao.TerminaPregao();

            // Act - método sob teste 
            leilao.RecebeLance(fulano, 1200);

            // Assert - Verificação das expectativas
            var qtdObtida = leilao.Lances.Count();
            Assert.Equal(qtdEsperada, qtdObtida);
        }
    

        [Theory]
        [InlineData(new double[] { 200, 300, 400, 500 })]
        [InlineData(new double[] { 200 })]
        [InlineData(new double[] { 200, 300, 400 })]
        [InlineData(new double[] { 200, 300, 400, 500, 600, 700 })]
        public void LeilaoAindaNaoFoiIniciado(double[] ofertas)
        {
            var avaliador = new MaiorValor();
            var leilao = new Leilao("Van Gogh", avaliador);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            for (int i = 0; i < ofertas.Length; i++)
            {
                var valor = ofertas[i];
                if ((i % 2) == 0)
                {
                    leilao.RecebeLance(fulano, valor);
                }
                else
                {
                    leilao.RecebeLance(maria, valor);
                }
            }
            Assert.Empty(leilao.Lances);

        }

        [Fact]
        public void NaoAceitaNovoLanceDadoInteressadoSerOMesmoQueDoUltimoLance()
        {
            // Arranje - cenário
            var avaliador = new MaiorValor();
            var leilao = new Leilao("Van Gogh", avaliador);
            var fulano = new Interessada("Fulano", leilao);
            leilao.IniciaPregao();
            leilao.RecebeLance(fulano, 1000);

            // Act - método sob teste 
            leilao.RecebeLance(fulano, 1200);

            // Assert - Verificação das expectativas
            var qtdEsperada = 1;
            var qtdObtida = leilao.Lances.Count();
            Assert.Equal(qtdEsperada, qtdObtida);
        }
    }
    
}
