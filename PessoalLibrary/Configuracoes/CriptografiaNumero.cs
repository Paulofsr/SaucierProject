using System;
using System.Collections.Generic;
using System.Text;

namespace PessoalLibrary.Configuracoes
{
    public static class CriptografiaNumero
    {
        private static List<string> _lista = new List<string>();
        private static List<string> _mascaras = new List<string>();
        private static Random _random = new Random();

        private static void InicializarLista()
        {
            if (_lista.Count == 0)
            {
                _lista.Add("a");
                _lista.Add("j");
                _lista.Add("c");
                _lista.Add("G");
                _lista.Add("g");
                _lista.Add("E");
                _lista.Add("i");
                _lista.Add("k");
                _lista.Add("e");
                _lista.Add("7");
                _lista.Add("r");
                _lista.Add("H");
                _lista.Add("d");
                _lista.Add("R");
                _lista.Add("o");
                _lista.Add("b");
                _lista.Add("f");
                _lista.Add("q");
                _lista.Add("4");
                _lista.Add("T");
                _lista.Add("2");
                _lista.Add("N");
                _lista.Add("t");
                _lista.Add("6");
                _lista.Add("O");
                _lista.Add("D");
                _lista.Add("u");
                _lista.Add("x");
                _lista.Add("l");
                _lista.Add("m");
                _lista.Add("F");
                _lista.Add("h");
                _lista.Add("5");
                _lista.Add("w");
                _lista.Add("J");
                _lista.Add("z");
                _lista.Add("C");
                _lista.Add("3");
                _lista.Add("B");
                _lista.Add("p");
                _lista.Add("S");
                _lista.Add("A");
                _lista.Add("n");
                _lista.Add("1");
                _lista.Add("I");
                _lista.Add("K");
                _lista.Add("s");
                _lista.Add("L");
                _lista.Add("0");
                _lista.Add("P");
                _lista.Add("8");
                _lista.Add("v");
                _lista.Add("Q");
                _lista.Add("U");
                _lista.Add("V");
                _lista.Add("W");
                _lista.Add("9");
                _lista.Add("X");
                _lista.Add("Y");
                _lista.Add("Z");
                _lista.Add("M");
                _lista.Add("y");

                _mascaras.Add("{0}f{1}U{2}6{3}r{4}R{5}o{6}");
                _mascaras.Add("{0}R{1}y{2}i{3}5{4}w{5}o{6}");
                _mascaras.Add("{0}n{1}S{2}q{3}I{4}9{5}o{6}");
            }
        }
        /*123 4567 890
         */

        private static string GetAlteracao(string texto, int qualParte)
        {
            int mod = texto.Length % 3;
            int inicio;
            if (mod == 0)
            {
                inicio = (texto.Length / 3) * qualParte;
                return Cript(texto.Substring(inicio, texto.Length / 3));
            }
            else
            {
                if (mod == 1)
                {
                    if (qualParte == 0)
                    {
                        inicio = ((texto.Length - 1) / 3) * qualParte;
                        return Cript(texto.Substring(inicio, (texto.Length - 1) / 3));
                    }
                    if (qualParte == 1)
                    {
                        inicio = ((texto.Length - 1) / 3) * qualParte;
                        return Cript(texto.Substring(inicio, ((texto.Length - 1) / 3) + 1));
                    }

                    inicio = (((texto.Length - 1) / 3) * qualParte) + 1;
                    return Cript(texto.Substring(inicio));
                }

                //Mod = 2 123 45 678
                if (qualParte == 0)
                {
                    inicio = ((texto.Length - 2) / 3) * qualParte;
                    return Cript(texto.Substring(inicio, ((texto.Length - 2) / 3) + 1));
                }
                if (qualParte == 1)
                {
                    inicio = (((texto.Length - 2) / 3) * qualParte) + 1;
                    return Cript(texto.Substring(inicio, (texto.Length - 2) / 3));
                }

                inicio = (((texto.Length - 2) / 3) * qualParte) + 1;
                return Cript(texto.Substring(inicio));
            }
            return string.Empty;
        }

        private static string Cript(string dados)
        {
            string result = string.Empty;
            for (int i = 0; i < dados.Length; i++)
                result += Mudar(dados[i]);
            //result=result.Replace(
            return result;
        }

        private static string Mudar(char p)
        {
            int posicao = Index(p);
            if (posicao > -1)
                if (posicao < _lista.Count - 1)
                    return _lista[posicao + 1];
                else
                    return _lista[0];
            return p.ToString();
        }

        private static int Index(char p)
        {
            for (int i = 0; i < _lista.Count; i++)
                if (_lista[i].Equals(p.ToString()))
                    return i;
            return -1;
        }

        private static string GetQualquer()
        {
            string result = _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)]
                + _lista[_random.Next(0, _lista.Count - 1)] + _lista[_random.Next(0, _lista.Count - 1)];
            return result.Replace("\\\\", "(E");
        }

        private static string Valores(string texto)
        {
            int mod = texto.Length % 3;
            int fator = (texto.Length - mod) / 3;
            return mod.ToString() + "." + fator.ToString() + ":";
        }

        /// <summary>
        /// Repasse o texto que deseja criptografá-lo.
        /// </summary>
        /// <param name="texto">Texto para codificá-lo.</param>
        /// <returns name="Criptografar">Texto criptografado.</returns>
        public static string Criptografar(string texto)
        {
            InicializarLista();

            return string.Format(_mascaras[_random.Next(0, _mascaras.Count - 1)], Valores(texto) + GetQualquer(), GetAlteracao(texto, 0), GetQualquer(), GetAlteracao(texto, 1), GetQualquer(), GetAlteracao(texto, 2), GetQualquer()) + "v001";

        }

        private static int GetMod(string texto)
        {
            return Convert.ToInt32(texto.Substring(0, texto.IndexOf(".")));
        }

        private static int GetFator(string texto)
        {
            int inicio = texto.IndexOf(".") + 1;
            return Convert.ToInt32(texto.Substring(inicio, texto.IndexOf(":") - inicio));
        }

        /// <summary>
        /// Descodifica textos criptografados pela essa regra.
        /// </summary>
        /// <param name="texto">Texto criptogradado.</param>
        /// <returns name="Return">Texto encontrado;</returns>
        public static string Descriptografar(string texto)
        {
            InicializarLista();
            try
            {
                string versao = texto.Substring(texto.Length - 4);
                string texto2 = texto.Substring(0, texto.Length - 4);
                if (versao == "v001")
                {
                    int mod = GetMod(texto2);
                    int fator = GetFator(texto2);

                    string primeiro = Descriptografra(texto2, mod, fator, 0);
                    string segundo = Descriptografra(texto2, mod, fator, 1);
                    string terceiro = Descriptografra(texto2, mod, fator, 2);

                    return primeiro + segundo + terceiro;
                }
            }
            catch { }
            return string.Empty;
        }

        private static string Descriptografra(string texto, int mod, int fator, int parte)
        {
            int numeroDeRandom = 48;
            int acrescimo = mod.ToString().Length + fator.ToString().Length + 2;//length de mode e do fator mais . e :
            //_mascaras.Add("{0}-{1}.{2}={3}]{4}R{5}/{6}");

            int primeiroAcrescimo = acrescimo + numeroDeRandom + 1;//acrescimo + {0} + -
            int segundoAcrescimo = 1 + numeroDeRandom + 1;// . + {2} + =
            int terceiroAcrescimo = 1 + numeroDeRandom + 1;// ] + {4} + R
            int ultimoAcrescimo = 1 + numeroDeRandom;// / + {6}

            try
            {
                if (mod == 0)
                {
                    if (parte == 0)//Primeira parte
                    {
                        string parteString = texto.Substring(primeiroAcrescimo, fator);
                        return Descript(parteString);
                    }
                    if (parte == 1)//Segunda parte
                    {
                        string parteString = texto.Substring(primeiroAcrescimo + fator + segundoAcrescimo, fator);
                        return Descript(parteString);
                    }
                    if (parte == 2)//Terceira parte
                    {
                        string parteString = texto.Substring(primeiroAcrescimo + fator + segundoAcrescimo + fator + terceiroAcrescimo, fator);
                        return Descript(parteString);
                    }
                }

                if (mod == 1)
                {
                    if (parte == 0)//Primeira parte
                    {
                        string parteString = texto.Substring(primeiroAcrescimo, fator);
                        return Descript(parteString);
                    }
                    if (parte == 1)//Segunda parte
                    {
                        string parteString = texto.Substring(primeiroAcrescimo + fator + segundoAcrescimo, fator + 1);
                        return Descript(parteString);
                    }
                    if (parte == 2)//Terceira parte
                    {
                        string parteString = texto.Substring(primeiroAcrescimo + fator + 1 + segundoAcrescimo + fator + terceiroAcrescimo, fator);
                        return Descript(parteString);
                    }
                }

                if (mod == 2)
                {
                    if (parte == 0)//Primeira parte
                    {
                        string parteString = texto.Substring(primeiroAcrescimo, fator + 1);
                        return Descript(parteString);
                    }
                    if (parte == 1)//Segunda parte
                    {
                        string parteString = texto.Substring(primeiroAcrescimo + fator + 1 + segundoAcrescimo, fator);
                        return Descript(parteString);
                    }
                    if (parte == 2)//Terceira parte
                    {
                        string parteString = texto.Substring(primeiroAcrescimo + fator + 1 + segundoAcrescimo + fator + terceiroAcrescimo, fator + 1);
                        return Descript(parteString);
                    }
                }
            }
            catch { }
            return string.Empty;
        }

        private static string Descript(string dados)
        {
            string result = string.Empty;
            for (int i = 0; i < dados.Length; i++)
                result += Voltar(dados[i]);
            return result;
        }

        private static string Voltar(char p)
        {
            int posicao = Index(p);
            if (posicao > -1)
            {
                if (posicao != 0)
                    return _lista[posicao - 1];
                else
                    return _lista[_lista.Count - 1];
            }
            return p.ToString();
        }
    }
}
