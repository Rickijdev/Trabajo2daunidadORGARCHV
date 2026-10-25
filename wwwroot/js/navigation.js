// PERSONA 2: comportamiento visual del menú. No accede a datos bancarios.
(() => {
    "use strict";
    const mobile = window.matchMedia("(max-width: 960px)");
    const storageKey = "bancoapp.navigation.compact";
    let compact = false, previousOverflow = "", lastShell = null, lastUrl = "";
    try { compact = localStorage.getItem(storageKey) === "true"; } catch { /* Preferencia opcional. */ }
    const shell = () => document.getElementById("banco-shell");
    const sidebar = () => document.getElementById("menu-principal");
    const isOpen = () => mobile.matches && !!shell()?.classList.contains("is-menu-open");
    function updateToggle() {
        const expanded = mobile.matches ? isOpen() : !compact;
        const label = mobile.matches ? (expanded ? "Cerrar menú" : "Abrir menú") : (expanded ? "Contraer menú" : "Expandir menú");
        document.querySelectorAll("[data-nav-toggle]").forEach(button => {
            button.setAttribute("aria-expanded", String(expanded));
            button.setAttribute("aria-label", label);
            button.setAttribute("title", label);
        });
    }
    function setMobileOpen(open, restoreFocus = true) {
        const root = shell(), menu = sidebar(), workspace = document.getElementById("banco-workspace");
        if (!root || !menu) return;
        open = open && mobile.matches;
        const wasOpen = root.classList.contains("is-menu-open");
        root.classList.toggle("is-menu-open", open);
        menu.inert = mobile.matches && !open;
        if (workspace) workspace.inert = open;
        if (mobile.matches) menu.setAttribute("aria-hidden", String(!open));
        else menu.removeAttribute("aria-hidden");
        if (open) {
            menu.setAttribute("role", "dialog");
            menu.setAttribute("aria-modal", "true");
            if (!wasOpen) previousOverflow = document.documentElement.style.overflow;
            document.documentElement.style.overflow = "hidden";
            (menu.querySelector(".menu-link.active") || menu.querySelector("a, button"))?.focus();
        } else {
            menu.removeAttribute("role");
            menu.removeAttribute("aria-modal");
            document.documentElement.style.overflow = previousOverflow;
            if (wasOpen && restoreFocus) document.querySelector("[data-nav-toggle]")?.focus();
        }
        updateToggle();
    }
    function initialize() {
        const root = shell();
        if (!root) return;
        root.classList.toggle("is-compact", compact);
        root.querySelectorAll(".menu-link").forEach(link => {
            if (link.classList.contains("active")) link.setAttribute("aria-current", "page");
            else link.removeAttribute("aria-current");
        });
        if (root !== lastShell || location.href !== lastUrl) setMobileOpen(false, false);
        lastShell = root; lastUrl = location.href;
        updateToggle();
    }
    document.addEventListener("click", event => {
        if (!(event.target instanceof Element)) return;
        if (event.target.closest("[data-nav-toggle]")) {
            if (mobile.matches) setMobileOpen(!isOpen());
            else {
                compact = !compact;
                shell()?.classList.toggle("is-compact", compact);
                try { localStorage.setItem(storageKey, String(compact)); } catch { /* Preferencia opcional. */ }
                updateToggle();
            }
            return;
        }
        if (event.target.closest("[data-nav-close]")) { setMobileOpen(false); return; }
        if (!event.ctrlKey && !event.metaKey && !event.shiftKey && event.button === 0 && event.target.closest("#menu-principal a") && isOpen()) setMobileOpen(false);
    });
    document.addEventListener("keydown", event => {
        if (!isOpen()) return;
        if (event.key === "Escape") { event.preventDefault(); setMobileOpen(false); }
        if (event.key !== "Tab") return;
        const items = Array.from(sidebar().querySelectorAll("a[href], button:not([disabled]), [tabindex='0']")).filter(item => item.getClientRects().length > 0);
        const first = items[0], last = items[items.length - 1];
        if (event.shiftKey && (document.activeElement === first || !sidebar().contains(document.activeElement))) { event.preventDefault(); last?.focus(); }
        else if (!event.shiftKey && (document.activeElement === last || !sidebar().contains(document.activeElement))) { event.preventDefault(); first?.focus(); }
    });
    mobile.addEventListener("change", () => setMobileOpen(false));
    initialize();
    if (window.Blazor?.addEventListener) window.Blazor.addEventListener("enhancedload", initialize);
    window.addEventListener("pageshow", initialize);
})();
