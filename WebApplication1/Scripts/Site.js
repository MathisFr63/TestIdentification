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
            '<input class="form-control text-box single-line" data-val="true" data-val-number="The field Int32 must be a number." data-val-required="Le champ Int32 est requis." name="' + name + '" type="number" min="0" value="1" require>' +
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
            '<td class="col-md-4">' +
            '<button type="reset" value="cancel" title="Supprimer un Produit" onclick="toggleArticleEdit(\'' + name + '\')" class="btn btn-danger">' +
            '<i class="glyphicon glyphicon-remove"></i >' +
            '</button>' +
            '</td>' +
            '</tr>' +
            '</table>';
        $("#articles").append(b);
    }
    else {
        document.getElementById(name + " input").checked = false;
        a.remove();
    }
}

var nb = 0;

function addNumero(value, prefixe) {
    if (!value)
        value = "";
    if (!prefixe)
        prefixe = "+33";
    var b = '<div id="' + nb + '">' +
            '<div style="display:flex;">' +
                '<div style="display: flex;">' +
                    '<input class="form-control text-box single-line" style= "width: 57px" type= "text" value= "' + prefixe + '" name="prefixe' + nb + '" required>' +
                    '<input class="form-control text-box single-line" id="item_Num_ro" name="' + nb + '" type="text" value="' + value + '" required>' +
                '</div>' +
                '<button type="reset" value="cancel" title="Créer un Nouveau Devis" onclick="removeNumero(parentNode.parentNode);" class="btn btn-danger" style="margin:10px">' +
                    '<i class="glyphicon glyphicon-remove"></i >' +
                '</button>' +
            '</div>' +
        '</div>';
    $("#telephones").append(b);
    nb++;
}

function removeNumero(elt) {
    elt.remove();
}

function avancée() {
    var div = document.getElementById("listAvancée");
    if (div.style.display == "none")
        div.style.display = "table-row";
    else
        div.style.display = "none";
}