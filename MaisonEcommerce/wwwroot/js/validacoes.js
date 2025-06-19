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

function anteriorImg() {
    let anterior = document.getElementById('imagens');
    const anteriorImg = document.getElementById('anteriorImg');

    if (anterior.src == 'http://localhost:5146/image/captcha/img1.png') anterior.src = '../image/captcha/img2.png';

    else if (anterior.src == 'http://localhost:5146/image/captcha/img2.png') anterior.src = '../image/captcha/img3.png';

    else if (anterior.src == 'http://localhost:5146/image/captcha/img3.png') anterior.src = '../image/captcha/img4.png';

    else if (anterior.src == 'http://localhost:5146/image/captcha/img4.png') anterior.src = '../image/captcha/img5.png';

    else if (anterior.src == 'http://localhost:5146/image/captcha/img5.png') anterior.src = '../image/captcha/img6.png';

    else if (anterior.src == 'http://localhost:5146/image/captcha/img6.png') anterior.src = '../image/captcha/img1.png';
}

function proximoImg() {
    let proximo = document.getElementById('imagens');

    if (proximo.src == 'http://localhost:5146/image/captcha/img1.png') proximo.src = '../image/captcha/img6.png';

    else if (proximo.src == 'http://localhost:5146/image/captcha/img2.png') proximo.src = '../image/captcha/img1.png';

    else if (proximo.src == 'http://localhost:5146/image/captcha/img3.png') proximo.src = '../image/captcha/img2.png';

    else if (proximo.src == 'http://localhost:5146/image/captcha/img4.png') proximo.src = '../image/captcha/img3.png';

    else if (proximo.src == 'http://localhost:5146/image/captcha/img5.png') proximo.src = '../image/captcha/img4.png';

    else if (proximo.src == 'http://localhost:5146/image/captcha/img6.png') proximo.src = '../image/captcha/img5.png';
}

function confirmarCaptcha() {
    let imagem = document.getElementById('imagens');
    let redefinir = document.getElementById('redefinir');
    let erro = document.getElementById('erro');
    let form = document.getElementById('captcha');
    let sucesso = document.getElementById('sucesso');

    if (imagem.src == 'http://localhost:5146/image/captcha/img2.png') {
        redefinir.disabled = false;
        erro.classList.add('d-none');
        form.classList.add('d-none');
        sucesso.classList.remove('d-none');
    }

    else erro.classList.remove('d-none');
}