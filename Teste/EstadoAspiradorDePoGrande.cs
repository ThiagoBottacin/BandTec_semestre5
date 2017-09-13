using System;
using System.Collections.Generic;
using System.Text;
using Busca;
using Busca.Grafo;
using Busca.Util;

namespace Teste
{
    class EstadoAspiradorDePoGrande : Estado
    {
        public const int LIMPO = 0;
        public const int SUJO = 1;

        public const int TAMANHO = 16;

        private readonly int[] quartos;
        private readonly int posicao, h;
        private int hashCode;
        private readonly string operacao;

        public EstadoAspiradorDePoGrande(int[] quartos, int posicao, string operacao)
        {
            this.quartos = quartos;
            this.posicao = posicao;
            this.operacao = operacao;
            h = CalcularH();
            // vamos iniciar o hash code com 0, e calculá-lo apenas quando for necessário
            hashCode = 0;
        }

        public override string ToString()
        {
            // para poder imprimir a solução completa na tela
            return operacao;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            EstadoAspiradorDePoGrande e = (obj as EstadoAspiradorDePoGrande);
            if (e == null || e.posicao != posicao)
            {
                return false;
            }
            for (int i = 0; i < TAMANHO; i++)
            {
                if (e.quartos[i] != quartos[i])
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            // aqui é necessário que o hash code já tenha sido calculado!!!
            if (hashCode == 0)
            {
                hashCode = CalcularHashCode();
            }
            return hashCode;
        }

        private int CalcularH()
        {
            int count = 0;

            foreach (var q in quartos)
                if (q == SUJO) count++;

            return count;
        }

        private int CalcularHashCode()
        {
            int h = posicao;
            for (int i = 0; i < TAMANHO; i++)
            {
                h = (h * 3) + quartos[i];
            }
            return h;
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
                return "Problema classico do aspirador de po (para varios quartos)";
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
                foreach (var q in quartos)
                    if (q == SUJO) return false;

                Predicate<int> match;

                match.Target(SUJO);

                Array.Exists(quartos, 1);

                return true;
            }
        }

        public IEnumerable<Estado> Sucessores
        {
            get
            {
                List<Estado> sucessores = new List<Estado>();

                int posicaoAtual = posicao != TAMANHO ? posicao : (posicao - 1);

                if (quartos[posicaoAtual] == SUJO)
                {
                    var q = (int[])quartos.Clone();
                    q[posicaoAtual] = LIMPO;

                    sucessores.Add(new EstadoAspiradorDePoGrande(q, posicaoAtual, "Limpar"));
                }

                if (posicaoAtual == 0)
                {
                    sucessores.Add(new EstadoAspiradorDePoGrande(quartos, (posicaoAtual + 1), "Direita"));
                }
                else if (posicaoAtual == (TAMANHO - 1))
                {
                    sucessores.Add(new EstadoAspiradorDePoGrande(quartos, (posicaoAtual - 1), "Esquerda"));
                }
                else
                {
                    sucessores.Add(new EstadoAspiradorDePoGrande(quartos, (posicaoAtual - 1), "Esquerda"));
                    sucessores.Add(new EstadoAspiradorDePoGrande(quartos, (posicaoAtual + 1), "Direita"));
                }

                return sucessores;
            }
        }
    }
}
