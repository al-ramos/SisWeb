
// Caminho: /Validators/CnpjValidator.cs
using System;
using System.Linq;

namespace SisWebCrud.Utils
{
    public static class CnpjValidator
    {
        public static bool ValidarCnpj(string cnpj)
        {
            // Remove caracteres não numéricos
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            // O CNPJ deve ter 14 dígitos e não ser formado por números repetidos
            if (cnpj.Length != 14 || cnpj.Distinct().Count() == 1)
                return false;

            int[] peso1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] peso2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int primeiroDV = CalcularDigitoVerificador(cnpj.Substring(0, 12), peso1);
            int segundoDV = CalcularDigitoVerificador(cnpj.Substring(0, 13), peso2);

            return (cnpj[12] - '0') == primeiroDV && (cnpj[13] - '0') == segundoDV;
        }

        private static int CalcularDigitoVerificador(string parcial, int[] peso)
        {
            int soma = parcial.Select((ch, idx) => (ch - '0') * peso[idx]).Sum();
            int resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }
    }
}

