setTimeout(function () {
    var selectedOnLoad = $(".readerPanelBox .highlight[data-ref]");
    if (selectedOnLoad.length > 0) {
        var segment = selectedOnLoad.eq(0).attr("data-ref");
        showIyunFor(segment);
    }
}, 500);

$(document).on('click', '[data-iyun-ref]', function (e) {
    var ref = $(this).attr('data-iyun-ref');
    var params = getUrlVars();

    params.p2 = ref;

    var newParams = $.param(params);

    location = window.location.pathname + "?" + newParams;
});

function getUrlVars() {
    var vars = {}, hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars[hash[0]] = hash[1];
    }
    return vars;
}

$(".readerContent").on('click', 'div.segment[data-ref]', function (e) {

    var segment = $(this).attr('data-ref');

    showIyunFor(segment);
});

function showIyunFor(segment) {
    if ($("#categoryFilterGroup").length <= 0) {
        $(".contentInner div:eq(1)")
            .append('<div class="categoryFilterGroup" id="categoryFilterGroup" style="border-top:4px solid rgb(151,173,134)"><div class="categoryFilter‏" style="font-size:17px;text-transform:uppercase;padding:17px 34px;cursor:pointer;line-height:20px"><span class="he">עיון</span><div class="iyun-links" style="padding-top:10px;"></div></div></div>')
    }

    $("#categoryFilterGroup div.iyun-links").empty();

    var url = "https://localhost:44360/api/links/" + segment;

    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            console.log(data);

            if (data.length > 0) {
                for (value of data) {
                    $("#categoryFilterGroup div.iyun-links")
                        .append(`<div style='cursor:pointer;line-height:20px; width:100%;padding-top:7px;padding-bottom:7px;'><a href='javascript:' data-iyun-ref='${value.sourceRef}'>${value.sourceHeRef}</a></div>`);
                }
            } else {
                $("#categoryFilterGroup div.iyun-links")
                    .append(`<div style='cursor:pointer;line-height:20px; width:100%;padding-top:7px;padding-bottom:7px;'>(אין הצעות)</div>`);
            }
        },
        error: function (e) {
            console.log('Error in Operation');
        }
    });
}
