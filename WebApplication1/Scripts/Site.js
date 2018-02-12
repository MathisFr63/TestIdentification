function showCheckboxes() {
    var checkboxes = document.getElementById("checkboxes");
    checkboxes.style.display = (checkboxes.style.display == "block") ? "none" : "block";
}

function toggleArticle(name) {
    var a = document.getElementById(name);
    if (!a) {
        var b = '<table class="table" id="' + name + '">' +
            '<tr>' +
            '<td class="col-md-4">' +
            '<label>Article</label>' +
            '<br />' +
            '<p>' + name + '</p>' +
            '</td>' +
            '<td class="col-md-4">' +
            '<label>Quantité</label>' +
            '<br />' +
            '<input class="form-control text-box single-line" data-val="true" data-val-number="The field Int32 must be a number." data-val-required="Le champ Int32 est requis." name="' + name + '" type="number" min="0" value="">' +
            '</td>' +
            '</tr>' +
            '</table>';
        $("#articles").append(b);
    }
    else {
        a.remove();
    }
}

function toggleArticleEdit(name, quantite) {
    var a = document.getElementById(name);
    if (!a) {
        var b = '<table class="table" id="' + name + '">' +
            '<tr>' +
            '<td class="col-md-4">' +
            '<label>Article</label>' +
            '<br />' +
            '<p>' + name + '</p>' +
            '</td>' +
            '<td class="col-md-4">' +
            '<label>Quantité</label>' +
            '<br />' +
            '<input class="form-control text-box single-line" data-val="true" data-val-number="The field Int32 must be a number." data-val-required="Le champ Int32 est requis." name="' + name + '" type="number" min="0" value="' + quantite + '">' +
            '</td>' +
            '</tr>' +
            '</table>';
        $("#articles").append(b);
    }
    else {
        a.remove();
    }
}