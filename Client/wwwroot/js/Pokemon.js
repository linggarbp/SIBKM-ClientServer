//$.ajax({
//    url: "https://pokeapi.co/api/v2/pokemon/",
//}).done((result) => {
//    var temp = "";
//    $.each(result.results, (key, val) => {
//        temp += `<tr>
//                    <td>${key + 1}</td>
//                    <td>${toTitleCase(val.name)}</td>
//                    <td><button type="button" class="btn btn-primary" onclick="detail('${val.url}');descData('${key + 1}');descMove('${key + 1}');" data-bs-toggle="modal" data-bs-target="#modalPokemon">
//                            Detail
//                        </button>
//                    </td>
//                 </tr>`
//    })
//    $("#tbodyPokemon").html(temp);
//}).fail((error) => {
//    console.log(error);
//})

$(document).ready(function () {
    $('#tablePokemon').DataTable({
        ajax: {
            url: 'https://pokeapi.co/api/v2/pokemon/',
            dataSrc: 'results'
        },
        columns: [
            {
                data: "no",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                data: "name",
                render: function (data, type, row) {
                    return data.charAt(0).toUpperCase() + data.slice(1);
                }
            },
            {
                data: "",
                render: function (data, type, row, meta) {
                    var number = meta.row + meta.settings._iDisplayStart + 1;
                    return `<button type = "button" class="btn btn-primary" onclick = "detail('${row.url}'); descData(${number}); descMove(${number});" data-bs-toggle="modal" data-bs-target="#modalPokemon">Detail</button>`;
                }
            }
        ]
    });
})

function descData(key) {
    $.ajax({
        url: stringUrl = 'https://pokeapi.co/api/v2/pokemon-species/' + key
    }).done((result) => {
        $("#descPokemon").text(result.flavor_text_entries[0].flavor_text);
    })
}

function descMove(key) {
    $.ajax({
        url: stringUrl = 'https://pokeapi.co/api/v2/move/' + key
    }).done((result) => {
        $("#moveType").text(`Type.................................${toTitleCase(result.type.name)}`);
        $("#statAccuracy").text(`Accuracy...............................${result.accuracy}`);
        $("#statPower").text(`Power.....................................${result.power}`);
        $("#statPP").text(`PP............................................${result.pp}`);
    })
}

function detail(stringUrl) {
    $.ajax({
        url: stringUrl
    }).done((result) => {
        $(".modal-title").html(toTitleCase(result.name));
        $("img").attr('src', result.sprites.other.dream_world.front_default);
        if (result.types[0].type.name == 'grass') {
            $(".types").html(toTitleCase(result.types[0].type.name));
            $(".types").css('background-color', "limegreen");
            $(".types").css('margin-left', "57px");
        }
        else if (result.types[0].type.name == 'water') {
            $(".types").html(toTitleCase(result.types[0].type.name));
            $(".types").css('background-color', "dodgerblue");
            $(".types").css('margin-left', "57px");
        }
        else if (result.types[0].type.name == 'fire') {
            $(".types").html(toTitleCase(result.types[0].type.name));
            $(".types").css('background-color', "crimson");
            $(".types").css('margin-left', "65px");
        }
        else if (result.types[0].type.name == 'bug') {
            $(".types").html(toTitleCase(result.types[0].type.name));
            $(".types").css('background-color', "darkorange");
            $(".types").css('margin-left', "65px");
        }
        else if (result.types[0].type.name == 'normal') {
            $(".types").html(toTitleCase(result.types[0].type.name));
            $(".types").css('background-color', "azure");
            $(".types").css('margin-left', "53px");
        }

        $("#statHp").text(`Hp.........................................${result.stats[0].base_stat}`);
        $("#statAttack").text(`Attack.................................${result.stats[1].base_stat}`);
        $("#statDefense").text(`Defense..............................${result.stats[2].base_stat}`);
        $("#statSpecAtk").text(`Special Attack..................${result.stats[3].base_stat}`);
        $("#statSpecDef").text(`Special Defense................${result.stats[4].base_stat}`);
        $("#statSpeed").text(`Speed..................................${result.stats[5].base_stat}`);
    })
}

function toTitleCase(str) {
    return str.replace(
        /\w\S*/g,
        function (txt) {
            return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
        }
    );
}