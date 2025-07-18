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
      blackOverlayClassList.toggle("opacity-full");
    }, 1);
  } else {
    aside.style.marginInlineStart = "-100%";
    blackOverlayClassList.toggle("opacity-full");
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
