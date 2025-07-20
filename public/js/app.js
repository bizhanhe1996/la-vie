const toggleTheme = (elm) => {
  const theme = document.querySelector("html").classList.item(1);
  const html = document.querySelector("html");
  html.classList = ["panel"];
  if (theme == "light") {
    html.classList.add("dark");
    elm.children[0].innerHTML = "light_mode";
    elm.children[1].innerHTML = "Light";
  } else {
    html.classList.add("light");
    elm.children[0].innerHTML = "dark_mode";
    elm.children[1].innerHTML = "Dark";
  }
};

const toggleAside = (target) => {
  target.parentNode.parentNode.parentNode.parentNode.classList.toggle(
    "aside-toggle"
  );
  target.children[0].classList.toggle("rotate-180");
};

const toggleDirection = (element) => {
  const html = document.querySelector("html");
  const direction = html.getAttribute("dir");
  if (direction == "rtl") {
    html.setAttribute("dir", "ltr");
    element.children[0].innerHTML = "format_textdirection_r_to_l";
    element.children[1].innerHTML = "RTL";
  } else {
    html.setAttribute("dir", "rtl");
    element.children[0].innerHTML = "format_textdirection_l_to_r";
    element.children[1].innerHTML = "LTR";
  }
};

const toggleAsideMobile = async () => {
  const aside = document.querySelector("aside");
  const leftValue = aside.style.marginInlineStart;
  const mode = leftValue == "0px" ? "toHide" : "toShow";
  const blackOverlayClassList =
    document.querySelector("#black-overlay").classList;

  if (mode == "toShow") {
    aside.style.marginInlineStart = "0px";
    blackOverlayClassList.toggle("block");
    window.setTimeout(() => {
      blackOverlayClassList.toggle("opacity-100");
    }, 1);
  } else {
    aside.style.marginInlineStart = "-100%";
    blackOverlayClassList.toggle("opacity-100");
    window.setTimeout(() => {
      blackOverlayClassList.toggle("block");
    }, 300);
  }
};

const toggleSubmenu = (elm) => {
  const height = elm.nextElementSibling.style.height;
  elm.nextElementSibling.style.height = height == "auto" ? "0" : "auto";
  [...elm.children].at(-1).classList.toggle("rotate-180");
};

const toggleConfirm = (message, elm) => {
  if(window.confirm(message)) {

    elm.querySelector('a').click()
  }
}


window.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll("div.tag-selector input").forEach((input) => {
    input.addEventListener("keydown", (event) => {
      const input = event.target;
      let query;
      if (event.key === "Enter") {
        event.preventDefault();
        query = input.value;
        const matches = [...input.nextElementSibling.children].filter(
          (option) => {
            return option.label.toLowerCase().includes(query.toLowerCase());
          }
        );
        if (matches.length === 1) {
          matches[0].selected = true;
          const selectedTagNode = document.createElement("span");
          selectedTagNode.classList.add("tag");
          selectedTagNode.innerHTML = matches[0].label;
          input.previousElementSibling.append(selectedTagNode);
          selectedTagNode.addEventListener("click", (event) => {
            event.target.parentNode.parentNode.querySelector(
              `select option[label=${selectedTagNode.innerHTML}]`
            ).selected = false;
            event.target.remove();
          });
          input.value = null;
        }
      } else if (event.key == "Backspace") {
        const candidate = [...input.previousElementSibling.children]?.at(-1);
        if (candidate) {
          input.parentNode.querySelector(
            `select option[label=${candidate.innerHTML}]`
          ).selected = false;
          candidate.remove();
        }
      }
    });
  });
});
