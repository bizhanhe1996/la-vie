export const toggleTheme = (activator) => {
  const html = document.querySelector("html");
  const currentTheme = html.classList.item(0);
  if (currentTheme == "light") {
    window.localStorage.setItem("la-vie-theme", "dark");
    html.classList.replace("light", "dark");
    activator.children[0].innerText = "light_mode";
    if (activator.id == "aside-theme-switch") {
      activator.children[1].innerHTML = "Light";
    }
  } else if (currentTheme == "dark") {
    window.localStorage.setItem("la-vie-theme", "light");
    html.classList.replace("dark", "light");
    activator.children[0].innerText = "dark_mode";
    if (activator.id == "aside-theme-switch") {
      activator.children[1].innerHTML = "Dark";
    }
  }
};

export const injectModuleHelpData = (moduleHelpData) => {
  const moduleHelp = document.querySelector("#module-help");
  moduleHelp.querySelector("#module-help-title").innerHTML =
    moduleHelpData.title;
  // foreach role
  for (const role of ["Admin", "Manager", "Client"]) {
    const tr = document.createElement("tr");
    const td = document.createElement("td");
    td.innerHTML = role;
    tr.appendChild(td);
    if (moduleHelpData["permissions"][role] === false) {
      for (let i in [0, 1, 2, 3]) {
        const emptyTd = document.createElement("td");
        tr.appendChild(emptyTd);
      }
    } else if (moduleHelpData["permissions"][role] === true) {
      for (let i in [0, 1, 2, 3]) {
        const emptyTd = document.createElement("td");
        emptyTd.innerHTML = "✓";
        tr.appendChild(emptyTd);
      }
    } else {
      // for each permission
      for (const [permission, value] of Object.entries(
        moduleHelpData["permissions"][role]
      )) {
        const td = document.createElement("td");
        if (value) {
          td.innerHTML = "✓";
        }
        tr.appendChild(td);
      }
    }
    moduleHelp.querySelector("tbody").appendChild(tr);
  }
  moduleHelp.querySelector("p#module-help-description").innerHTML =
    moduleHelpData.description;
};

export const toggleModuleHelp = (target) => {
  // variables
  const blackOverlay = document.querySelector("#black-overlay");
  const moduleHelp = document.querySelector("#module-help");
  const action = moduleHelp.classList.contains("module-help-opened")
    ? "hide"
    : "show";
  if (action == "show") {
    blackOverlay.addEventListener("click", toggleModuleHelp);
    moduleHelp.classList.add("module-help-opened");
  } else if (action == "hide") {
    blackOverlay.removeEventListener("click", toggleModuleHelp);
    moduleHelp.classList.remove("module-help-opened");
  }
  toggleBlackOverlay();
};

export const toggleAside = (target) => {
  target.parentNode.parentNode.parentNode.parentNode.classList.toggle(
    "aside-toggle"
  );
  target.children[0].classList.toggle("rotate-180");
};

export const toggleDirection = (element) => {
  const html = document.querySelector("html");
  const direction = html.getAttribute("dir");
  if (direction == "rtl") {
    html.setAttribute("dir", "ltr");
    window.localStorage.setItem("la-vie-direction", "ltr");
    element.children[0].innerHTML = "format_textdirection_r_to_l";
    element.children[1].innerHTML = "RTL";
  } else {
    html.setAttribute("dir", "rtl");
    window.localStorage.setItem("la-vie-direction", "rtl");
    element.children[0].innerHTML = "format_textdirection_l_to_r";
    element.children[1].innerHTML = "LTR";
  }
};

export const toggleAsideMobile = async () => {
  const aside = document.querySelector("aside");
  const leftValue = aside.style.marginInlineStart;
  const action = leftValue == "0px" ? "hide" : "show";
  if (action == "show") {
    document
      .querySelector("#black-overlay")
      .addEventListener("click", toggleAsideMobile);
  } else if (action == "hide") {
    document
      .querySelector("#black-overlay")
      .removeEventListener("click", toggleAsideMobile);
  }
  aside.style.marginInlineStart = action == "show" ? "0" : "-100%";
  toggleBlackOverlay();
};

export const toggleBlackOverlay = () => {
  const blackOverlay = document.querySelector("#black-overlay");
  const action = blackOverlay.classList.contains("block") ? "hide" : "show";
  if (action === "show") {
    blackOverlay.classList.add("block");
    window.setTimeout(() => {
      blackOverlay.classList.add("opacity-100");
    }, 1);
  } else if (action === "hide") {
    blackOverlay.classList.remove("opacity-100");
    window.setTimeout(() => {
      blackOverlay.classList.remove("block");
    }, 300);
  }
};

export const toggleSubmenu = (elm) => {
  const maxHeight = elm.nextElementSibling.style.maxHeight;
  elm.nextElementSibling.style.maxHeight = maxHeight == "256px" ? "0" : "256px";
  [...elm.children].at(-1).classList.toggle("rotate-180");
};

export const toggleConfirm = (message, elm) => {
  if (window.confirm(message)) {
    elm.querySelector("a").click();
  }
};

export const changePageSize = (controller, selectElement) => {
  window.localStorage.setItem("pagination-size", selectElement.value);
  window.location.href =
    "/" + controller + "?page=1&size=" + selectElement.value;
};

export const allowDrop = (event) => {
  event.preventDefault();
};

export const multiDelete = async (module, tableId) => {
  if (
    window.confirm("Are you sure that you want to delete all selected rows?")
  ) {
    const deletableIds = [
      ...document.querySelectorAll(
        `table#${tableId} tbody tr td div.checkbox input:checked`
      ),
    ].map((checkbox) => {
      return +checkbox.getAttribute("data-id");
    });

    debugger;
    const response = await fetch("/Category/MultiDelete", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        RequestVerificationToken: window.sessionStorage.getItem("CSRF-token"),
      },
      body: JSON.stringify(deletableIds),
    });

    const result = await response.json();

    if (result.deletedCount > 0) {
      window.location.href = result.redirectUrl;
    } else {
      window.alert("Nothing deleted!");
    }
  }
};
