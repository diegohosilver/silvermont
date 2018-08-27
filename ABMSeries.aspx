<%@ Page Title="" Language="C#" MasterPageFile="~/SilverMont.master" AutoEventWireup="true" CodeFile="ABMSeries.aspx.cs" Inherits="ABMSeries" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
            <script type="text/javascript">
                $(function ($) {
                    var node = document.getElementById("nav").getElementsByTagName("li")[4];
                    node.setAttribute("class", "current");
                    if (document.getElementById('<%= hfPanelMostrar.ClientID%>').value == '0') {
                        $('.collapsible-panels div').hide();
                    }
                    else if (document.getElementById('<%= hfPanelMostrar.ClientID%>').value == '1') {
                        $('.collapsible-panels div').show();
                    }
                });
                function abrirCerrar() {
                    $('.collapsible-panels div').slideToggle(350);
                }
                function ConfirmDelete() {
                    var delete_value = document.createElement("INPUT");
                    delete_value.type = "hidden";
                    delete_value.name = "delete_value";
                    if (confirm("Esta seguro de eliminar la serie?")) {
                        delete_value.value = "Yes";
                    } else {
                        delete_value.value = "No";
                    }
                    document.forms[0].appendChild(delete_value);
                }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="main-wrapper">
		    <div id="main" class="container">
                    <div class="row 200%">
					        <div class="12u">
					            <div>
                                    <br />
                                      <h2 class="major"><span title="Click para desplegar" style="cursor:pointer; cursor:hand;" onclick="abrirCerrar();">Datos de la Serie</span></h2>
                                        <div class="12u">
								            <div class="content">
                                                <div class="collapsible-panels">                                            
									            <!-- Content -->
                                                    <div>
										            <article class="box page-content">
                                                        <section class="box highlight">
                                                         <header>
                                                             <p style="display:inline-block">Nombre de la serie<asp:TextBox ID="txtTituloSerie" runat="server" style="margin-left:10px; margin-right:50px; width:400px"></asp:TextBox>Creador<asp:TextBox ID="txtCreador" runat="server" style="margin-left:10px; width:400px;"></asp:TextBox></p>
                                                             <br />
                                                             <br />
                                                             <p style="display:inline-block">Fecha de estreno<asp:TextBox ID="txtFechaEstreno" class="datePicker" runat="server" style="margin-left:10px; margin-right:50px; width:150px"></asp:TextBox>
                                                                 Genero<asp:DropDownList ID="ddlGenero" runat="server" style="margin-left:10px; margin-right:50px;"></asp:DropDownList>
                                                                 Pais<asp:DropDownList ID="ddlPais" runat="server" style="margin-left:10px;"></asp:DropDownList>
                                                             </p>
                                                             <br />
                                                             <br />                                                      
                                                             <p>Reseña</p>
                                                             <asp:TextBox ID="txtReseña" runat="server" TextMode="Multiline" style="Resize:none; width:1150px; height:200px"></asp:TextBox>
                                                             <br />
                                                             <br />
                                                             <p style="display:inline-block; ">Imagen<asp:FileUpload ID="fuImagen" runat="server" style="margin-left:10px; margin-right:50px" />Miniatura<asp:FileUpload ID="fuMiniatura" runat="server" style="margin-left:10px;" /></p>
                                                             <br />
                                                             <br />
                                                             <p style="display:inline-block; ">Cantidad de temporadas<asp:TextBox ID="txtCantidadTemporadas" runat="server" style="margin-left:10px; width:150px;"></asp:TextBox></p>
                                                             <br />
                                                             <br />
                                                             <asp:Button ID="btnGuardar" runat="server" Text="Guardar Serie" OnClick="guardarSerie_Click" />
                                                             <asp:Button ID="btnModificar" runat="server" Text="Guardar Serie" OnClick="modificarSerie_Click" Visible="False" />
                                                             <br />
                                                             <br /> 
                                                             <p><asp:Label ID="lblError" runat="server" Visible="false" style="color:red"></asp:Label></p>                                                              
                                                         </header>
                                                       </section>                                                     
										            </article>
                                                    </div>                                                                                                 
                                                </div>
								            </div>
							            </div>
                                      <h2 class="major"><span style="cursor:default;">Listado de series</span></h2>
                                    <center>
                                        <asp:GridView ID="gvNoticias" runat="server" HorizontalAlign="Center" AllowPaging="True" PageSize="4" OnPageIndexChanging="gvNoticias_PageIndexChanging"
                                            OnRowCommand="gvNoticias_OnRowCommand"
                                            CssClass="Grid"  
                                            Width="95%" BorderColor="Black"            
                                              AlternatingRowStyle-CssClass="alt"
                                              PagerStyle-CssClass="pgr">
                                            <RowStyle HorizontalAlign="Center" Wrap="False"  Font-Size="15px"/>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnModificar" ToolTip="Editar Serie" ImageUrl="/images/btnEditarSmall.png" runat="server" CommandName="MODIFICAR" CommandArgument='<%# Eval("id")%>'/>
                                                        <asp:ImageButton ID="btnEliminar" ToolTip="Eliminar Serie" ImageUrl="/images/btnBorrarSmall.png" runat="server" CommandName="ELIMINAR" OnClientClick="ConfirmDelete();" CommandArgument='<%# Eval("id")%>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <SelectedRowStyle Wrap="True" />
                                            <HeaderStyle CssClass="GrillasHeaders" ForeColor="Black" />
                                            <AlternatingRowStyle CssClass="GrillasFilaAlternate" />
                                        </asp:GridView>
                                    </center>
					                </div>
						        </div>
				            </div>
                        </div>
                    </div>
    <asp:HiddenField ID="hfPanelMostrar" runat="server" />
    <asp:HiddenField ID="hfIDSerie" runat="server" />
</asp:Content>

