function cpf(input) {
    let qtd = input.value;

    if (qtd.length == 3 || qtd.length == 7) qtd += '.';

    if (qtd.length == 11) qtd += '-';

    input.value = qtd;
}

function numero(input) {
    input.value = input.value.replace(/\D/g, '');

}

function texto(input) {
    input.value = input.value.replace(/[0-9]/g, '');
}

function telefone(input) {
    let qtd = input.value;

    if (qtd.length == 0) qtd += '(';

    if (qtd.length == 3) qtd += ')';

    if (qtd.length == 8) qtd += '-';

   
    input.value = qtd;
}

function decimal(input) {
    input.value = input.value.replace(/[^\d,]/g, '');
}

function mostrarSenha() {
    let senha = document.getElementById('senha');
    let olho = document.getElementById('olho');

    if (senha.type == 'password') {
        senha.type = 'text';
        olho.src = "../image/OlhoAberto.png";
        olho.style.transition = 'all 0.5s;';
    }

    else {
        senha.type = 'password';
        olho.src = "../image/OlhoFechado.png";
    }
}