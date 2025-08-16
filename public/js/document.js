window.addEventListener("DOMContentLoaded", () => {
  // loading theme
  const theme = window.localStorage.getItem("la-vie-theme");
  if (theme === null) {
    window.localStorage.setItem("la-vie-theme", "light");
  } else {
    document.querySelector("html").classList.replace("theme", theme);
    document.querySelector("html").style.visibility = "visible";
  }

  // pagination size
  document.querySelectorAll("select#pagination-size").forEach((select) => {
    select.value = window.localStorage.getItem("pagination-size");
  });

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

  // table checkboxes
  document.querySelectorAll("th div.checkbox input").forEach((checkbox) => {
    checkbox.addEventListener("change", (event) => {
      event.target
        .closest("table")
        .querySelectorAll("tbody div.checkbox input")
        .forEach((input) => {
          input.checked = event.target.checked;
        });
    });
  });

  document.querySelectorAll("td div.checkbox input").forEach((checkbox) => {
    checkbox.addEventListener("change", () => {
      checkbox
        .closest("table")
        .querySelector("thead th div.checkbox input").checked = false;
    });
  });

  // adding CSRF token to session storage
  const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
  const token = tokenInput.value;
  window.sessionStorage.setItem("CSRF-token",token);
  tokenInput.remove();

});
