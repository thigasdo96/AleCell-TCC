// ============================================================
// ALECELL — Admin JavaScript v2
// ============================================================

document.addEventListener('DOMContentLoaded', function () {

  // ---- SIDEBAR TOGGLE (mobile) ----
  const toggleBtn = document.getElementById('sidebarToggle');
  const sidebar   = document.getElementById('adminSidebar');
  const mainArea  = document.getElementById('adminMain');

  if (toggleBtn && sidebar) {
    toggleBtn.addEventListener('click', function () {
      if (window.innerWidth <= 900) {
        sidebar.style.display = sidebar.style.display === 'block' ? 'none' : 'block';
      } else {
        sidebar.classList.toggle('collapsed');
        if (mainArea) mainArea.classList.toggle('expanded');
      }
    });

    document.addEventListener('click', function (e) {
      if (
        window.innerWidth <= 900 &&
        sidebar.style.display === 'block' &&
        !sidebar.contains(e.target) &&
        !toggleBtn.contains(e.target)
      ) {
        sidebar.style.display = 'none';
      }
    });
  }

  // ---- CONFIRMAÇÃO VIA data-confirm ----
  document.querySelectorAll('[data-confirm]').forEach(function (el) {
    el.addEventListener('click', function (e) {
      if (!confirm(el.dataset.confirm)) e.preventDefault();
    });
  });

  // ---- ALERTAS AUTO-DISMISS ----
  document.querySelectorAll('.alert').forEach(function (alerta) {
    setTimeout(function () {
      alerta.style.transition = 'opacity 0.4s ease, transform 0.4s ease';
      alerta.style.opacity    = '0';
      alerta.style.transform  = 'translateY(-4px)';
      setTimeout(() => alerta.remove(), 400);
    }, 5000);
  });

  // ---- PREVIEW DE COR (categorias) ----
  const inputCor = document.querySelector('input[name="Cor"]');
  if (inputCor) {
    const preview = document.createElement('div');
    preview.style.cssText = `
      width: 32px; height: 32px; border-radius: 6px;
      border: 1px solid rgba(255,255,255,0.1);
      display: inline-block; vertical-align: middle;
      margin-left: .5rem; background: ${inputCor.value || '#888'};
    `;
    inputCor.insertAdjacentElement('afterend', preview);
    inputCor.addEventListener('input', function () { preview.style.background = inputCor.value; });
  }

  // ---- ESTATÍSTICAS: anima contadores ----
  document.querySelectorAll('.stat-valor[data-target]').forEach(function (el) {
    const target = parseInt(el.dataset.target) || 0;
    let current  = 0;
    const step   = Math.max(1, Math.floor(target / 30));
    const timer  = setInterval(function () {
      current = Math.min(current + step, target);
      el.textContent = current.toLocaleString('pt-BR');
      if (current >= target) clearInterval(timer);
    }, 30);
  });

});
