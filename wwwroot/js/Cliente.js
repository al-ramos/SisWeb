$(document).ready(function () {
    $("form").validate();
    console.log("$.fn.validate:", $.fn.validate); // Verifique se a função validate existe

    // Função para preencher o campo DataCadastro com a data atual
    function preencherDataCadastro() {
        var dataCadastroInput = $('#DataCadastro');
        if (dataCadastroInput.length) { // Verifica se o elemento existe na página
            var today = new Date().toISOString().slice(0, 10);
            dataCadastroInput.val(today);
        }
    }

    // Chama a função para preencher a data
    preencherDataCadastro();

    function tratarDocumento() {
        console.log("tratarDocumento chamado. Tipo:", $('#TipoCliente').val(), "Documento:", $('#Documento').val());
        var tipo = $('#TipoCliente').val();
        var documentoInput = $('#Documento');

        documentoInput.unmask();
        documentoInput.rules("remove");
        documentoInput.removeClass("input-validation-error");
        documentoInput.next("span.text-danger").empty();

        if (tipo === 'PF') {
            documentoInput.mask('999.999.999-99');
            documentoInput.rules("add", {
                required: true,
                cpf: true,
                messages: {
                    required: "O CPF é obrigatório.",
                    cpf: "Por favor, digite um CPF válido."
                }
            });
        } else if (tipo === 'PJ') {
            documentoInput.mask('99.999.999/9999-99');
            documentoInput.rules("add", {
                required: true,
                cnpj: true,
                messages: {
                    required: "O CNPJ é obrigatório.",
                    cnpj: "Por favor, digite um CNPJ válido."
                }
            });
        }
        $("form").validate().element(documentoInput); // Valida o campo
    }

    $('#TipoCliente').change(tratarDocumento).trigger('change'); // Chama no change e no carregamento
    $('#Documento').blur(tratarDocumento);

    console.log('Evento blur anexado ao #Documento');

    $.validator.addMethod("cpf", function (value, element) {
        console.log("Validando CPF:", value);
        value = value.replace(/\D/g, "");
        console.log("CPF sem máscara:", value);
        if (value.length !== 11) {
            console.log("CPF com comprimento incorreto:", value.length);
            return false;
        }
        // ... outros logs ...
        var sum = 0;
        var remainder;
        for (var i = 1; i <= 9; i++) {
            console.log("i:", i, "value.substring(i - 1, i):", value.substring(i - 1, i), "(11 - i):", (11 - i));
            sum = sum + parseInt(value.substring(i - 1, i)) * (11 - i);
        }
        console.log("Soma (primeiro dígito):", sum);
        remainder = (sum * 10) % 11;
        console.log("Resto (primeiro dígito):", remainder);
        if ((remainder === 10) || (remainder === 11)) remainder = 0;
        console.log("Resto ajustado (primeiro dígito):", remainder);
        if (remainder !== parseInt(value.substring(9, 10))) {
            console.log("Primeiro dígito verificador incorreto:", remainder, parseInt(value.substring(9, 10)));
            return false;
        }
        // Repita para o segundo dígito
        // ...
        return true;
    }, "Por favor, digite um CPF válido.");

    $.validator.addMethod("cnpj", function (value, element) {
        // Remove tudo que não é dígito
        value = value.replace(/[^\d]+/g, "");

        // O CNPJ precisa ter 14 dígitos
        if (value.length !== 14) return false;

        // Elimina CNPJs inválidos conhecidos (ex.: todos dígitos iguais)
        if (/^(\d)\1+$/.test(value)) return false;

        // Validação do primeiro dígito verificador
        var tamanho = 12;
        var numeros = value.substring(0, tamanho);
        var digitos = value.substring(tamanho);
        var soma = 0;
        var pos = tamanho - 7; // inicia com 5 (12 - 7 = 5)

        for (var i = 0; i < tamanho; i++) {
            soma += parseInt(numeros.charAt(i)) * pos;
            pos = pos === 2 ? 9 : pos - 1;
        }

        var resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
        if (resultado != digitos.charAt(0)) return false;

        // Validação do segundo dígito verificador
        tamanho = tamanho + 1; // agora considera os 13 dígitos (incluindo o primeiro dígito verificador)
        numeros = value.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7; // inicia com 6 (13 - 7 = 6)

        for (var i = 0; i < tamanho; i++) {
            soma += parseInt(numeros.charAt(i)) * pos;
            pos = pos === 2 ? 9 : pos - 1;
        }

        resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
        if (resultado != digitos.charAt(1)) return false;

        return true;
    }, "Por favor, digite um CNPJ válido.");


    // Inicialize a validação do formulário
    $("form").validate();
});