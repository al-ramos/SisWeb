using System;
using System.Linq;

namespace SisWebCrud.Validators
{
    public static class CpfValidator
    {
        public static bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove caracteres não numéricos
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // O CPF deve ter 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais (ex: 11111111111)
            if (cpf.Distinct().Count() == 1)
                return false;

            // Calcula o primeiro dígito verificador
            string baseCpf = cpf.Substring(0, 9);
            int primeiroDigito = CalcularDigito(baseCpf, 10);

            // Calcula o segundo dígito verificador
            int segundoDigito = CalcularDigito(baseCpf + primeiroDigito, 11);

            // Verifica se os dígitos calculados correspondem aos dígitos informados
            return cpf.EndsWith($"{primeiroDigito}{segundoDigito}");
        }

        private static int CalcularDigito(string baseCpf, int pesoInicial)
        {
            int soma = 0;
            foreach (char ch in baseCpf)
            {
                soma += (ch - '0') * pesoInicial;
                pesoInicial--;
            }
            int resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }
    }
}
