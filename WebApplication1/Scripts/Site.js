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

var nb = 0;

function addNumero(value) {
    if (!value)
        value = "";
    var b = '<div id="' + nb + '">' +
        '<label> Numéro </label>' +
        '<div style="display:flex;">' +
        '<input class="form-control text-box single-line" id="item_Num_ro" name="' + nb + '" required="required" type="text" value="' + value + '">' +
        '<button type="reset" value="cancel" title="Créer un Nouveau Devis" onclick="removeNumero(parentNode.parentNode);" class="btn btn-danger" style="margin:10px">'+
        '<i class="glyphicon glyphicon-remove"></i >'+
        '</button>'+
        '</div>' +
        '</div>';
    $("#telephones").append(b);
    nb++;
}

function removeNumero(elt) {
    elt.remove();
}