/* ------------------------------------------------------------------------
Class: freezeHeader
Use:freeze header row in html table
Example 1:  $('#tableid').freezeHeader();
Example 2:  $("#tableid").freezeHeader({ 'height': '300px' });
Example 3:  $("table").freezeHeader();
Example 4:  $(".table2").freezeHeader();
Author: Laerte Mercier Junior
Version: 1.0.5
-------------------------------------------------------------------------*/
(function ($) {
    var TABLE_ID = 0;
    $.fn.freezeHeader = function (params) {

        var copiedHeader = false;

        function freezeHeader(elem) {
            var idObj = ('tbl-' + (++TABLE_ID));
            //var idObj = elem.attr('id') || ('tbl-' + (++TABLE_ID));
            if (elem.length > 0 && elem[0].tagName.toLowerCase() == "table") {

                var obj = {
                    id: idObj,
                    grid: elem,
                    container: null,
                    header: null,
                    divScroll: null,
                    openDivScroll: null,
                    closeDivScroll: null,
                    scroller: null
                };

                if (params && params.height !== undefined) {
                    obj.divScroll = '<div id="hdScroll' + obj.id + '" style="height: ' + params.height + '; overflow-y: scroll">';
                    obj.closeDivScroll = '</div>';
                }

                obj.header = obj.grid.find('thead');

                if (params && params.height !== undefined) {
                    if ($('#hdScroll' + obj.id).length == 0) {
                        obj.grid.wrapAll(obj.divScroll);
                    }
                }

                obj.scroller = params && params.height !== undefined
				   ? $('#hdScroll' + obj.id)
				   : $(window);

                obj.scroller.on('scroll', function () {

                    if ($('#hd' + obj.id).length == 0) {
                        obj.grid.before('<div id="hd' + obj.id + '" class="staticheadertableextenddiv"></div>');
                    }

                    obj.container = $('#hd' + obj.id);

                    if (obj.header.offset() != null) {
                        if (limiteAlcancado(obj, params)) {
                            if (!copiedHeader) {
                                cloneHeaderRow(obj);
                                copiedHeader = true;
                            }
                        }
                        else {

                            if (($(document).scrollTop() > obj.header.offset().top)) {
                                obj.container.css("position", "absolute");
                                obj.container.css("top", (obj.grid.find("tr:last").offset().top - obj.header.height()) + "px");
                            }
                            else {
                                obj.container.css("visibility", "hidden");
                                obj.container.css("top", "0px");
                                obj.container.width(0);
                            }
                            copiedHeader = false;
                        }
                    }

                });
            }
        }

        function limiteAlcancado(obj, params) {
            if (params && params.height !== undefined) {
                return (obj.header.offset().top <= obj.scroller.offset().top);
            }
            else {
                return ($(document).scrollTop() > obj.header.offset().top && $(document).scrollTop() < (obj.grid.height() - obj.header.height() - obj.grid.find("tr:last").height()) + obj.header.offset().top);
            }
        }

        function cloneHeaderRow(obj) {
            obj.container.html('');
            obj.container.val('');
            var tabela = $('<table></table>');
            var atributos = obj.grid.prop("attributes");

            $.each(atributos, function () {
                if (this.name != "id") {
                    tabela.attr(this.name, this.value);
                }
            });

            var headheight = obj.grid.find("thead").first().height() || 0;

            //tabela.append('<thead class="bg-primary">' + obj.header.html() + '</thead>');
            var tablestr = '';
            //var tabletrlist = obj.grid.find('tr');
            //debugger;
            //for (var ii = 0; ii < tabletrlist.length; ii++) {
            //    if (ii == 0) {
            //        tablestr += '<thead class="bg-primary"><tr>' + $(tabletrlist[ii]).html() + '</tr></thead><tbody style="visibility:hidden;">';
            //    }
            //    else if (ii == tabletrlist.length-1) {
            //        tablestr += '<tr>' + $(tabletrlist[ii]).html() + '</tr>';
            //        break;
            //    }
            //}
            obj.grid.find('tr').each(function (index) {
                if (index == 0) {
                    tablestr += '<thead class="bg-primary"><tr>' + $(this).html() + '</tr></thead><tbody style="visibility:hidden;">';
                }
                else {
                    tablestr += '<tr>' + $(this).html() + '</tr>';
                }
            });

            tablestr += '</tbody>';
            tabela.append(tablestr);
            obj.container.append(tabela);
            obj.container.width(obj.header.width());
            obj.container.height(obj.header.height);
            //obj.container.find('th').each(function (index) {
            //    var cellWidth = obj.grid.find('th').eq(index).width();
            //    $(this).css('width', cellWidth);
            //});

            obj.container.css("visibility", "visible");

            if (params && params.height !== undefined) {
                obj.container.css("top", obj.scroller.offset().top + "px");
                obj.container.css("position", "absolute");
            } else {
                obj.container.css("top", "50px");
                obj.container.css("position", "fixed");
            }
            obj.container.css("height", headheight);
            obj.container.css("overflow", "hidden");
            obj.grid.parent().scroll(function () {
                obj.container.css("left", ((maxbox?49:299) - $(this).scrollLeft()) + "px")
            });
            $(".orderbytableclass").click(function () {
                var that = this;
                tableOrderBy(that);
            });
        }

        return this.each(function (i, e) {
            freezeHeader($(e));
        });

    };
})(jQuery);
