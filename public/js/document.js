window.addEventListener("DOMContentLoaded", () => {
  // tag selector
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

  // pagination size
  document.querySelectorAll("select#pagination-size").forEach((select) => {
    select.value = window.localStorage.getItem("pagination-size");
  });

  // loading theme
  const theme = window.localStorage.getItem("la-vie-theme");
  if (theme === null) {
    window.localStorage.setItem("la-vie-theme", "light");
  } else {
    document.querySelector('html').classList.replace("theme", theme);
    document.querySelector('html').style.visibility = "visible";
  }
});
