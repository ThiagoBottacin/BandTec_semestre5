using System;
using System.Collections.Generic;
using System.Text;
using Busca;
using Busca.Grafo;
using Busca.Util;

namespace Teste
{
    class EstadoHLAC : Estado
    {
        public const int MARGEM_INICIAL = 0;
        public const int MARGEM_FINAL = 1;

        private readonly int homem, lobo, carneiro, alface, h;
        private readonly string operacao;

        public EstadoHLAC(int homem, int lobo, int carneiro, int alface, string operacao)
        {
            this.homem = homem;
            this.lobo = lobo;
            this.carneiro = carneiro;
            this.alface = alface;
            this.operacao = operacao;
            h = CalcularH();
        }

        public override string ToString()
        {
            // para poder imprimir a solução completa na tela, incluindo também
            // quem estava de qual lado
            string margemInicial = "";
            string margemFinal = "";
            if (homem == MARGEM_INICIAL)
            {
                margemInicial += "H";
            }
            else
            {
                margemFinal += "H";
            }
            if (lobo == MARGEM_INICIAL)
            {
                margemInicial += "L";
            }
            else
            {
                margemFinal += "L";
            }
            if (carneiro == MARGEM_INICIAL)
            {
                margemInicial += "C";
            }
            else
            {
                margemFinal += "C";
            }
            if (alface == MARGEM_INICIAL)
            {
                margemInicial += "A";
            }
            else
            {
                margemFinal += "A";
            }
            return margemInicial + "|" + margemFinal + " (" + operacao + ")";
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            EstadoHLAC e = (obj as EstadoHLAC);
            return (e != null &&
                e.homem == homem &&
                e.lobo == lobo &&
                e.carneiro == carneiro &&
                e.alface == alface);
        }

        public override int GetHashCode()
        {
            // quando o hash code for "simples", ele pode ser calculado diretamente,
            // sem ser armazenado em uma variável
            return (homem * 11) + (lobo * 7) + (carneiro * 5) + (alface * 3);
        }

        private int CalcularH()
        {
            return 4 - (homem + lobo + carneiro + alface);
        }

        public int Custo
        {
            get
            {
                // todas as nossas operações têm o mesmo custo: 1
                // (não existe operação mais complexa que outra, ou mais
                // demorada que outra)
                return 1;
            }
        }

        public string Descricao
        {
            get
            {
                return
                    "Uma pessoa, um lobo, um carneiro e um cesto de alface estao a beira " +
                    "de um rio. Dispondo de um barco no qual pode carregar apenas a pessoa e " +
                    "mais um dos outros tres, a pessoa deve transportar tudo para a outra margem. " +
                    "Determine uma serie de travessias que respeite a seguinte condicao: " +
                    "em nenhum momento devem ser deixados juntos, sem a supervisao da pessoa, " +
                    "o lobo e o carneiro ou o carneiro e o cesto de alface.";
            }
        }

        public int H
        {
            get
            {
                return h;
            }
        }

        public bool IsMeta
        {
            get
            {
                return (h == 0);
            }
        }

        public IEnumerable<Estado> Sucessores
        {
            get
            {
                List<Estado> sucessores = new List<Estado>();

                EstadoHLAC novoEstado;

                // possíveis estados futuros
                novoEstado = TentarLevarLobo();
                if (novoEstado != null)
                {
                    sucessores.Add(novoEstado);
                }

                novoEstado = TentarLevarCarneiro();
                if (novoEstado != null)
                {
                    sucessores.Add(novoEstado);
                }

                novoEstado = TentarLevarAlface();
                if (novoEstado != null)
                {
                    sucessores.Add(novoEstado);
                }

                novoEstado = TentarNavegar();
                if (novoEstado != null)
                {
                    sucessores.Add(novoEstado);
                }

                return sucessores;
            }
        }
       

        #region Tentativas de Estados
        private EstadoHLAC TentarLevarLobo()
        {
            EstadoHLAC novoEstado = null;

            if (homem == MARGEM_INICIAL && lobo == MARGEM_INICIAL)
            {
                // tenta levar o Lobo da margem inicial para a margem final
                novoEstado = new EstadoHLAC(homem + 1, lobo + 1, carneiro, alface, "Levar o Lobo para a margem final");
            }
            else if (homem == MARGEM_FINAL && lobo == MARGEM_FINAL)
            {
                novoEstado = new EstadoHLAC(homem - 1, lobo - 1, carneiro, alface, "Levar o Lobo para a margem inicial");
            }

            if (novoEstado != null && novoEstado.IsEstadoValido)
            {
                return novoEstado;
            }
            return null;
        }

        private EstadoHLAC TentarLevarCarneiro()
        {
            EstadoHLAC novoEstado = null;

            if (homem == MARGEM_INICIAL && carneiro == MARGEM_INICIAL)
            {
                // tenta levar o Carneiro da margem inicial para a margem final
                novoEstado = new EstadoHLAC(homem + 1, lobo, carneiro + 1, alface, "Levar o Carneiro para a margem final");
            }
            else if (homem == MARGEM_FINAL && carneiro == MARGEM_FINAL)
            {
                novoEstado = new EstadoHLAC(homem - 1, lobo, carneiro - 1, alface, "Levar o Carneiro para a margem inicial");
            }

            if (novoEstado != null && novoEstado.IsEstadoValido)
            {
                return novoEstado;
            }
            return null;
        }

        private EstadoHLAC TentarLevarAlface()
        {
            EstadoHLAC novoEstado = null;

            if (homem == MARGEM_INICIAL && alface == MARGEM_INICIAL)
            {
                // tenta levar o Alface da margem inicial para a margem final
                novoEstado = new EstadoHLAC(homem + 1, lobo, carneiro, alface + 1, "Levar o Alface para a margem final");
            }
            else if (homem == MARGEM_FINAL && alface == MARGEM_FINAL)
            {
                novoEstado = new EstadoHLAC(homem - 1, lobo, carneiro, alface - 1, "Levar o Alface para a margem inicial");
            }

            if (novoEstado != null && novoEstado.IsEstadoValido)
            {
                return novoEstado;
            }
            return null;
        }

        private EstadoHLAC TentarNavegar()
        {
            EstadoHLAC novoEstado = null;

            if (homem == MARGEM_INICIAL)
            {
                // tenta levar o Alface da margem inicial para a margem final
                novoEstado = new EstadoHLAC(homem + 1, lobo, carneiro, alface, "Navegar para a margem final");
            }
            else if (homem == MARGEM_FINAL)
            {
                novoEstado = new EstadoHLAC(homem - 1, lobo, carneiro, alface, "Navegar para a margem inicial");
            }

            if (novoEstado != null && novoEstado.IsEstadoValido)
            {
                return novoEstado;
            }
            return null;
        }

        private bool IsEstadoValido
        {
            get
            {
                if ((carneiro == alface && homem != carneiro && lobo != carneiro) || (lobo == carneiro && homem != lobo && alface != lobo))
                {
                    return false;
                }

                return true;
            }
        }
        #endregion
    }
}
