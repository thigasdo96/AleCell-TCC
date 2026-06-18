// ============================================================
// ALECELL — JavaScript Principal v2
// ============================================================

document.addEventListener('DOMContentLoaded', function () {

  // ---- MENU MOBILE ----
  const menuToggle = document.getElementById('menuToggle');
  const navLinks   = document.getElementById('navLinks');

  if (menuToggle && navLinks) {
    menuToggle.addEventListener('click', function () {
      navLinks.classList.toggle('aberto');
    });
    document.addEventListener('click', function (e) {
      if (!menuToggle.contains(e.target) && !navLinks.contains(e.target)) {
        navLinks.classList.remove('aberto');
      }
    });
  }

  // ---- FAQ ACCORDION ----
  window.toggleFaq = function (btn) {
    const item    = btn.parentElement;
    const resp    = item.querySelector('.faq-resposta');
    const icon    = btn.querySelector('.faq-icon');
    const aberto  = resp.classList.contains('aberto');

    document.querySelectorAll('.faq-resposta.aberto').forEach(r => r.classList.remove('aberto'));
    document.querySelectorAll('.faq-icon').forEach(i => i.textContent = '+');

    if (!aberto) { resp.classList.add('aberto'); icon.textContent = '−'; }
  };

  // ---- ALERTAS AUTO-DISMISS ----
  document.querySelectorAll('.alert').forEach(function (alerta) {
    setTimeout(function () {
      alerta.style.transition = 'opacity 0.4s ease, transform 0.4s ease';
      alerta.style.opacity    = '0';
      alerta.style.transform  = 'translateY(-6px)';
      setTimeout(() => alerta.remove(), 400);
    }, 4500);
  });

  // ---- SELETOR DE ESTRELAS ----
  const labels = document.querySelectorAll('.estrela-label');
  labels.forEach(function (label, index) {
    label.addEventListener('mouseenter', function () {
      labels.forEach((l, i) => l.querySelector('span').style.opacity = i <= index ? '1' : '0.25');
    });
    label.addEventListener('mouseleave', function () {
      const checked = document.querySelector('.estrela-label input:checked');
      const ci      = checked ? parseInt(checked.value) - 1 : -1;
      labels.forEach((l, i) => l.querySelector('span').style.opacity = i <= ci ? '1' : '0.25');
    });
    label.addEventListener('click', function () {
      const val = parseInt(label.querySelector('input').value) - 1;
      labels.forEach((l, i) => l.querySelector('span').style.opacity = i <= val ? '1' : '0.25');
    });
  });

  // ---- PARCELAS — highlight no radio ----
  document.querySelectorAll('.parcela-opcao input').forEach(function (input) {
    input.addEventListener('change', function () {
      document.querySelectorAll('.parcela-card').forEach(function (card) {
        card.style.borderColor = '';
        card.style.background  = '';
      });
      const card = input.nextElementSibling;
      if (card) {
        card.style.borderColor = 'var(--primario)';
        card.style.background  = 'var(--primario-bg)';
      }
    });
  });

  // ---- LOGO — pulso sutil de brilho ----
  const logo = document.querySelector('.logo');
  if (logo) {
    setInterval(function () {
      logo.style.textShadow = '0 0 24px rgba(0,229,255,0.9), 0 0 48px rgba(0,229,255,0.35)';
      setTimeout(() => { logo.style.textShadow = ''; }, 180);
    }, 4000);
  }

  // ---- ANIMAÇÃO DE ENTRADA DOS CARDS ----
  const observer = new IntersectionObserver(function (entries) {
    entries.forEach(function (entry) {
      if (entry.isIntersecting) {
        entry.target.style.opacity   = '1';
        entry.target.style.transform = 'translateY(0)';
        observer.unobserve(entry.target);
      }
    });
  }, { threshold: 0.08 });

  document.querySelectorAll('.produto-card, .servico-card, .stat-card, .diferencial-item, .marca-card').forEach(function (el) {
    el.style.opacity    = '0';
    el.style.transform  = 'translateY(18px)';
    el.style.transition = 'opacity 0.38s ease, transform 0.38s ease';
    observer.observe(el);
  });

  // ---- SMOOTH HIGHLIGHT NA NAVBAR ----
  const currentPath = window.location.pathname.toLowerCase();
  document.querySelectorAll('.nav-links a').forEach(function (link) {
    const href = link.getAttribute('href');
    if (href && href !== '/' && currentPath.startsWith(href.toLowerCase())) {
      link.style.color = 'var(--primario)';
    }
  });

  // ---- MÁSCARA DE TELEFONE ----
  document.querySelectorAll('input[name="telefone"]').forEach(function (input) {
    input.addEventListener('input', function () {
      let v = input.value.replace(/\D/g, '').slice(0, 11);
      if (v.length > 10)
        v = v.replace(/^(\d{2})(\d{5})(\d{4})$/, '($1) $2-$3');
      else if (v.length > 6)
        v = v.replace(/^(\d{2})(\d{4})(\d*)$/, '($1) $2-$3');
      else if (v.length > 2)
        v = v.replace(/^(\d{2})(\d*)$/, '($1) $2');
      input.value = v;
    });
  });

  // ---- MÁSCARA DE CEP ----
  document.querySelectorAll('input[name="cep"]').forEach(function (input) {
    input.addEventListener('input', function () {
      let v = input.value.replace(/\D/g, '').slice(0, 8);
      if (v.length > 5) v = v.replace(/^(\d{5})(\d*)$/, '$1-$2');
      input.value = v;
    });
  });

});

// ---- USER MENU TOGGLE (chamado pelo _LoginPartial) ----
function toggleUserMenu(btn) {
  const menu = btn.parentElement;
  menu.classList.toggle('open');
  document.addEventListener('click', function closeMenu(e) {
    if (!menu.contains(e.target)) {
      menu.classList.remove('open');
      document.removeEventListener('click', closeMenu);
    }
  });
}
