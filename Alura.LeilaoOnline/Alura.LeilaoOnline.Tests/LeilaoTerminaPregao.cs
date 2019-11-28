using System;
using Alura.LeilaoOnline.Core;
using Xunit;

namespace Alura.LeilaoOnline.Tests
{
    public class LeilaoTerminaPregao
    {
        [Fact]
        private void RetornaErroSeTentarEncerradoDadoOPregaoNaoFoiIniciado()
        {
            // Arranje - cenário
            var avaliador = new MaiorValor();
            var leilao = new Leilao("Van Gogh", avaliador);

            // Act - método sob teste 
            var excecaoObtida = Assert.Throws<InvalidOperationException>(() => 
                // Assert - Verificação das expectativas 
                leilao.TerminaPregao()
            );

            var msgEsperada = "Não é possível terminar o pregão sem que ele tenha começado. Para isso, utilize o método IniciaPregao().";
            Assert.Equal(msgEsperada, excecaoObtida.Message);
        }

        [Fact] 
        private void RetornaZeroDadoLeilaoSemLance()
        {
            // Arranje - cenário
            var avaliador = new MaiorValor();
            var leilao = new Leilao("Van Gogh", avaliador);
            leilao.IniciaPregao();

            // Act - método sob teste 
            leilao.TerminaPregao();

            // Assert - Verificação das expectativas
            var valorEsperado = 0;
            var valorObtido = leilao.Ganhador.Valor;
            Assert.Equal(valorEsperado, valorObtido);
        }
        [Theory]
        [InlineData(1000, new double[] { 800, 900, 1000, 990 })]
        [InlineData(1200, new double[] { 800, 900, 1000, 1200 })]
        [InlineData(800, new double[] { 800 })]
        public void RetornaMaiorValorDadoLeilaoComPeloMenosUmLance(double valorEsperado, double[] ofertas)
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

            // Act - método sob teste 
            leilao.TerminaPregao();

            // Assert - Verificação das expectativas
            Assert.Equal(valorEsperado, leilao.Ganhador.Valor);
        }

        [Theory]
        [InlineData(1250, 1200, new double[] {800,1150,1250,1400})]
        [InlineData(1399, 1300, new double[] {900,1399,1450,1900})]
        public void RetornaValorSuperiorMaisProximoDadoLeilaoNessaModalidade(double valorEsperado, double valorDestino, double[] ofertas )
        {
            // Arranje - cenário
            IModalidadeAvaliacao modalidade =
                new OfertaSuperiorMaisProxima(valorDestino);
            var leilao = new Leilao("Van Gogh", modalidade);
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

            // Act - método sob teste 
            leilao.TerminaPregao();

            // Assert - Verificação das expectativas
            Assert.Equal(valorEsperado, leilao.Ganhador.Valor);
        }
    }
}
