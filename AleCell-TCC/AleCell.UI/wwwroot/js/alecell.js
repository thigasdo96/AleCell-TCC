// ============================================================
// ALECELL — JavaScript Principal v3 (refined)
// ============================================================

document.addEventListener('DOMContentLoaded', function () {

  // ================================================================
  // MENU MOBILE
  // ================================================================
  const menuToggle = document.getElementById('menuToggle');
  const navLinks   = document.getElementById('navLinks');

  if (menuToggle && navLinks) {
    menuToggle.addEventListener('click', function () {
      navLinks.classList.toggle('aberto');
      menuToggle.setAttribute('aria-expanded', navLinks.classList.contains('aberto'));
    });
    document.addEventListener('click', function (e) {
      if (!menuToggle.contains(e.target) && !navLinks.contains(e.target)) {
        navLinks.classList.remove('aberto');
      }
    });
  }

  // ================================================================
  // ACTIVE NAV LINK — destaca o link da página atual
  // ================================================================
  const currentPath = window.location.pathname.toLowerCase();
  document.querySelectorAll('.nav-links a').forEach(function (link) {
    const href = link.getAttribute('href');
    if (!href) return;
    // Home exata
    if (href === '/' && (currentPath === '/' || currentPath === '')) {
      link.classList.add('nav-ativo');
    } else if (href !== '/' && currentPath.startsWith(href.toLowerCase())) {
      link.classList.add('nav-ativo');
    }
  });

  // ================================================================
  // SCROLL-REVEAL — fade-in escalonado para cards
  // ================================================================
  const revealObserver = new IntersectionObserver(function (entries) {
    entries.forEach(function (entry) {
      if (entry.isIntersecting) {
        entry.target.classList.add('visible');
        revealObserver.unobserve(entry.target);
      }
    });
  }, { threshold: 0.07 });

  // Adiciona .reveal e stagger por posição no grid
  const revealSelectors = '.produto-card, .servico-card, .stat-card, .diferencial-item, .marca-card';
  document.querySelectorAll(revealSelectors).forEach(function (el) {
    el.classList.add('reveal');

    const siblings = el.parentElement ? Array.from(el.parentElement.children).filter(c => c.classList.contains(el.classList[0])) : [];
    const idx = siblings.indexOf(el);
    if (idx >= 0) {
      el.style.transitionDelay = Math.min(idx * 0.065, 0.42) + 's';
    }
    revealObserver.observe(el);
  });

  // ================================================================
  // FAQ — accordion suave via max-height (CSS-driven)
  // ================================================================
  window.toggleFaq = function (btn) {
    const item   = btn.parentElement;
    const resp   = item.querySelector('.faq-resposta');
    const icon   = btn.querySelector('.faq-icon');
    const aberto = resp.classList.contains('aberto');

    // Fecha todos
    document.querySelectorAll('.faq-resposta.aberto').forEach(function (r) {
      r.classList.remove('aberto');
    });
    document.querySelectorAll('.faq-icon').forEach(function (i) {
      i.textContent = '+';
      i.style.transform = '';
    });

    if (!aberto) {
      resp.classList.add('aberto');
      icon.textContent = '−';
      icon.style.transform = 'rotate(0deg)';
    }
  };

  // ================================================================
  // ALERTAS — auto-dismiss com fade
  // ================================================================
  document.querySelectorAll('.alert').forEach(function (alerta) {
    setTimeout(function () {
      alerta.style.transition = 'opacity 0.45s ease, max-height 0.45s ease, padding 0.45s ease';
      alerta.style.opacity    = '0';
      alerta.style.maxHeight  = '0';
      alerta.style.padding    = '0';
      alerta.style.overflow   = 'hidden';
      setTimeout(() => alerta.remove(), 460);
    }, 4500);
  });

  // ================================================================
  // LOGO — pulso sutil de brilho a cada 6s
  // ================================================================
  const logo = document.querySelector('.logo');
  if (logo) {
    setInterval(function () {
      logo.style.transition = 'text-shadow 0.15s ease';
      logo.style.textShadow = '0 0 30px rgba(0,255,0,1), 0 0 60px rgba(0,255,0,0.5)';
      setTimeout(() => { logo.style.textShadow = ''; }, 220);
    }, 6000);
  }

  // ================================================================
  // PARCELAS — highlight no radio
  // ================================================================
  document.querySelectorAll('.parcela-opcao input').forEach(function (input) {
    input.addEventListener('change', function () {
      // CSS já cuida via :checked, mas mantemos para compat.
      document.querySelectorAll('.parcela-card').forEach(function (card) {
        card.style.borderColor = '';
        card.style.background  = '';
        card.style.boxShadow   = '';
      });
    });
  });

  // ================================================================
  // SELETOR DE ESTRELAS
  // ================================================================
  const labels = document.querySelectorAll('.estrela-label');
  labels.forEach(function (label, index) {
    label.addEventListener('mouseenter', function () {
      labels.forEach((l, i) => {
        const span = l.querySelector('span');
        if (span) span.style.opacity = i <= index ? '1' : '0.25';
      });
    });
    label.addEventListener('mouseleave', function () {
      const checked = document.querySelector('.estrela-label input:checked');
      const ci      = checked ? parseInt(checked.value) - 1 : -1;
      labels.forEach((l, i) => {
        const span = l.querySelector('span');
        if (span) span.style.opacity = i <= ci ? '1' : '0.25';
      });
    });
    label.addEventListener('click', function () {
      const val = parseInt(label.querySelector('input').value) - 1;
      labels.forEach((l, i) => {
        const span = l.querySelector('span');
        if (span) span.style.opacity = i <= val ? '1' : '0.25';
      });
    });
  });

  // ================================================================
  // MÁSCARA DE TELEFONE
  // ================================================================
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

  // ================================================================
  // MÁSCARA DE CEP
  // ================================================================
  document.querySelectorAll('input[name="cep"]').forEach(function (input) {
    input.addEventListener('input', function () {
      let v = input.value.replace(/\D/g, '').slice(0, 8);
      if (v.length > 5) v = v.replace(/^(\d{5})(\d*)$/, '$1-$2');
      input.value = v;
    });
  });

  // ================================================================
  // SMOOTH IMAGE LOAD — fade in quando imagem carrega
  // ================================================================
  document.querySelectorAll('.produto-imagem').forEach(function (img) {
    if (img.complete) {
      img.style.opacity = '1';
    } else {
      img.style.opacity = '0';
      img.style.transition = 'opacity 0.35s ease';
      img.addEventListener('load', function () {
        img.style.opacity = '1';
      });
    }
  });

});

// ================================================================
// USER MENU TOGGLE (chamado pelo _LoginPartial)
// ================================================================
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
