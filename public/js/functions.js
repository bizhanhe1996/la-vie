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

export const toggleSmallAside = (target) => {
  const main = document.querySelector("body > main");
  const action = main.classList.contains("aside-small") ? "open" : "collpase";
  if (action == "open") {
    main.classList.remove("aside-small");
    window.localStorage.setItem("la-vie-aside-small", false);
  } else if (action == "collpase") {
    main.classList.add("aside-small");
    window.localStorage.setItem("la-vie-aside-small", true);
  }
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

export const toggleConfirm = (message, callback = null) => {
  const confirmModal = document.querySelector("div#la-vie-confirm-modal");
  const action = message ? "open" : "close";
  if (action == "open") {
    confirmModal.querySelector("p").innerHTML = message;
    ["opacity-100", "scale-100"].forEach((kls) =>
      confirmModal.classList.add(kls)
    );
    confirmModal.querySelector("button.yes").addEventListener("click", () => {
      callback();
      toggleConfirm(null);
    });
  } else if (action == "close") {
    confirmModal.querySelector("p").innerHTML = null;
    ["opacity-100", "scale-100"].forEach((kls) =>
      confirmModal.classList.remove(kls)
    );
    const yesButtonClone = confirmModal.querySelector("button.yes").cloneNode();
    yesButtonClone.innerHTML = "Yes";
    confirmModal.querySelector("button.yes").remove();
    confirmModal.querySelector("div.buttons").append(yesButtonClone);
  }
  toggleBlackOverlay();
};

export const changePageSize = (controller, pageSize) => {
  window.localStorage.setItem("pagination-size", pageSize);
  window.location.href = "/" + controller + "?page=1&size=" + pageSize;
};

export const allowDrop = (event) => {
  event.preventDefault();
};

export const toggleDropdown = (dropdown) => {
  ["scale-100", "opacity-100"].forEach((kls) => dropdown.classList.toggle(kls));
};

export const multiDelete = async (module, tableId) => {
  const deletableIds = [
    ...document.querySelectorAll(
      `table#${tableId} tbody tr td div.checkbox input:checked`
    ),
  ].map((checkbox) => {
    return +checkbox.getAttribute("data-id");
  });

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
};

export const addNotification = (type, title, message) => {
  const notificationTemplate = `
      <span></span>
      <div>
        <i class="material-icons"></i>
        <h3></h3>
      </div>
      <p></p>
  `;
  const notificationElement = document.createElement("div");
  notificationElement.classList.add("la-vie-notification");
  notificationElement.innerHTML = notificationTemplate;
  notificationElement.querySelector("h3").innerHTML = title;
  notificationElement.querySelector("p").innerHTML = message;
  const iconElement = notificationElement.querySelector("i");
  switch (type) {
    case "success":
      iconElement.innerHTML = "done_outline";
      notificationElement.classList.add("bg-success");
      break;
    case "error":
      iconElement.innerHTML = "error";
      notificationElement.classList.add("bg-danger");
      break;
    case "warning":
      iconElement.innerHTML = "warning";
      notificationElement.classList.add("bg-warning");
      break;
    case "info":
    default:
      iconElement.innerHTML = "notifications";
      notificationElement.classList.add("bg-primary");
      break;
  }

  notificationElement.addEventListener("click", function () {
    this.classList.add("!opacity-0");
    window.setTimeout(() => {
      this.remove();
    }, 500);
  });

  const underlayInterval = window.setInterval(() => {
    let currentWidth = notificationElement.querySelector("span").style.width;
    if (currentWidth == "") {
      currentWidth = "100";
    }
    currentWidth = +currentWidth.replace("%", "");
    notificationElement.querySelector("span").style.width =
      currentWidth - 1 + "%";
  }, 20);

  window.setTimeout(() => {
    window.clearInterval(underlayInterval);
    notificationElement.click();
  }, 2000);

  document
    .querySelector("#la-vie-notifications")
    .appendChild(notificationElement);

  window.setTimeout(() => {
    notificationElement.classList.add("!translate-x-0");
  }, 1);
};
