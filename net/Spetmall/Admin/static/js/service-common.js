
/**
 *@上传附件
 */
var upload_attachment = function(_config){
	
	var that=this,config={maxsize:3,upload_url: '/attachment.php/Manage/upload_image.html',delete_url:'/attachment.php/Manage/upload_delete.html'};
	this.message
	
	for(var _key in _config){
		config[_key] = _config[_key];
	}
	
	
	
	this.bind = function(){
		
	    $("#" + config['fileid'] ).bind('change',function(){
			if(!that.check()){
				alert(that.message);
				return ;
			}
			that.upload();
		});
	}
	
	this.setUploadUrl = function(_upload_url){
		config['upload_url'] = _upload_url;
		return this;
	}
	
	this.setDeleteUrl = function(_delete_url){
		config['delete_url'] = _delete_url;
		return this;
	}
	
	this.setContainer = function(_Element){
	    config['container'] = $(_Element);
		return this;
	}
	
	this.setFile = function(_fileid){
	    config['fileid'] = _fileid;
		return this;
	}
	
	this.callback = function(_data){
		create_item(_data);
	}
	
	this.check = function(){
	    if( $(".upload-images-item").size() >= config['maxsize'] ){
			that.message = "最多只能上传" + config['maxsize'];
			return false;
		}
		
		return true;
	}
	
	this.del = function(_hash){
	     
		 $.get(config['delete_url'],{"hash" : _hash },function(_result){
		      if(_result && !_result['status']){
			  	  alert(_result['msg']);
			  }
		 },'JSON');
		
	};
	
	
	this.parse_str = function(_str,_data){
	    return _str.replace(/\{#([\w]+)\}/g,function(_a,_b){
			return _data[_b] || '';
		});
	};
	
	function create_item(_data){
		_DOM = $( that.parse_str(that.template(),_data) );
		_DOM.bind('click',function(){
		     del($(this));
		});
		$( config['container'] ) .append(_DOM);
	};
	
	function del(_that){
		_that.find('.delete-icon').closest('li').remove();
		that.del(_that.find('.delete-icon').attr('data-hash'));
	};

	this.template=function(){
	  return '<li class="upload-images-item"><input type="hidden" name="upload_hash_data[]" value="{#hash}" /><input type="hidden" name="upload_pictures[]" value="{#filepath}" /><div class="img" style="background-image:url({#filepath});"></div><span class="delete-icon" data-hash="{#hash}"></span></li>';
	
	};
	
	this.upload = function(){
		console.log('upload');
	    var formData = new FormData();
	    formData.append("Filedata", document.getElementById(config['fileid']).files[0]);   
		$.ajax({
			url :  config['upload_url'] ,
			type: "POST",
			data: formData,
			/**
			*必须false才会自动加上正确的Content-Type
			*/
			contentType: false,
			/**
			* 必须false才会避开jQuery对 formdata 的默认处理
			* XMLHttpRequest会对 formdata 进行正确的处理
			*/
			processData: false,
			success: function (_result) {
				if ( _result.status == true   ) {
					that.callback(_result.data);
				}else{
					alert(_result.msg);
				}
				
			},
			error: function () {
				alert('上传异常');
			}
		});
	};
	
}





/**
 *附件管理 AND 上传
 */
var yunmallAttachmentManage = function(){
		
		this.windowdialog = null,that = this;
		
		 /*上传附件*/
		this.UploadAttr = function(callBack,_dom){
			that.windowdialog =  art.dialog.open("/attachment.php/Manage/upload.html"+"?filekey="+_dom.attr("filekey"),{
				  title:'附件管理',
				  width:654,
				  height:457,
				  cancel: true,
				  init: function () {
						var frame = this.iframe.contentWindow;
							frame.uploadcommon.setDom(_dom);
							/*frame.uploadcommon.setUploadType(uploadtype);
							frame.uploadcommon.setUploadSize(size);
							frame.uploadcommon.setFilekey(filekey);*/
							frame.uploadcommon.run();
				  },
				  ok:function(){
						 var frame = this.iframe.contentWindow;
						 frame.uploadcommon.callBack(callBack);
				  }
			});
		}
		
		this.bind = function(element){
			common_upload_image(element);
		};
		
		this.bindCutPictures = function(element){
			common_cut_pictures(element);	 
		};
		
		function common_upload_image(element){
			     element.attr({uploadtype:'jpg,jpeg,gif,png,bmp',filekey:'imagetype'});
				 that.UploadAttr(function(_data){
					 var cropping = getCropping(element);
					 
					 if(cropping!=false && _data.length==1 ){
						var imageinfo  = _data[0];
						imageinfo['cropwidth']   = cropping['cropwidth'];
						imageinfo['cropheight']  = cropping['cropheight']; 
						that.cut_pictures_dialog(imageinfo,function(_imgdata){
							setImageDom(element,[_imgdata]);
						});
						
					 }else{
						 setImageDom(element,_data);
					 } 
				},element);
		};
		
		function common_cut_pictures(element){
			 var cropping = getCropping(element);
			 if( cropping != false ){
			 	 var imageinfo = [];
				 imageinfo['url'] = $.trim( element.attr('data-src') );
			     imageinfo['cropwidth']   = cropping['cropwidth'];
				 imageinfo['cropheight']  = cropping['cropheight']; 
				
				 that.cut_pictures_dialog(imageinfo,function(_imgdata){
					setImageDom(element,[_imgdata]);
				 });
		     }		
		};
		
		/**
		 *设置上传图片地址
		 */
		function setImageDom(element,_data){
			 var _mapimageList = element.attr('yun_attachment').split("|");
			 for(var i in  _data){
					for(var j=0;j<_mapimageList.length;j++){
						$(_mapimageList[j]).each(function(index,obj) {
							var _dom =  $(this);
							if(_dom.is('input')){
								_dom.val(_data[i]['appurl']);
							}else if(_dom.is('img')){
								_dom.attr('src',_data[i]['appurl']+"?=" + new Date().getTime()).show();
							}else{
								_dom.attr('data-attachment-src',_data[i]['appurl']+"?=" + new Date().getTime()).show();
							}
						});
					}
			 }
		}
		
		
		//获取裁剪图片信息
		function getCropping(_that){
			 var cropping = _that.attr('cropping');
			 
			 if(typeof(cropping) != 'undefined' &&  cropping !='false' ){
					try{ 
					  cropping = jQuery.parseJSON( cropping );
					}catch(error){ 
					  cropping = true;
					}
					
					var imageinfo = {};
					if(cropping instanceof Array){
						imageinfo['cropwidth']   = cropping[0];
						imageinfo['cropheight']  = cropping[1]; 
					}else{
						imageinfo['cropwidth']  = 0;
						imageinfo['cropheight'] = 0;
					}
					return imageinfo;
			 }
			 return false;
		}

		
		/*开始截图*/
		function upload(_url,_data){
			$.post( _url ,_data,function(data){
				  if(windowdialog){ windowdialog.close();}
				  that.callBack.call(this,data);
			},'json');
		}
	
		this.cut_pictures_dialog = function(imageinfo,callBack){
	     var _url="/attachment.php/Manage/imagecropping.html";
		 that.windowdialog =  art.dialog.open(_url+(_url.indexOf('?')<0?'?':'&')+"imgurl="+$.trim(imageinfo['url'])+"&cropwidth="+imageinfo['cropwidth']+"&cropheight="+imageinfo['cropheight'],{
			  title:'图片裁剪',
			  width:'100%',
			  height:'100%',
			  fixed:true,
			  lock: true,
			  background: '#000', // 背景色
			  opacity: 0.4,
			  cancel: true,
			  init: function () {
					var frame = this.iframe.contentWindow;
						//frame.uploadcommon.setDom(_dom);
						/*frame.uploadcommon.setUploadType(uploadtype);
						frame.uploadcommon.setUploadSize(size);
						frame.uploadcommon.setFilekey(filekey);*/
		        	    //frame.uploadcommon.run();
			  },
			  ok:function(){
					 var frame= this.iframe.contentWindow;
					 var form = $(frame.document.getElementById('form'));
                     var data = {};
					 data['x']      = form.find("input[name=x]").val();
					 data['y']      = form.find("input[name=y]").val();
					 data['w']      = form.find("input[name=w]").val();
					 data['h']      = form.find("input[name=h]").val();
					 data['imgurl'] = form.find("input[name=imgurl]").val();
					 data['attrid'] = form.find("input[name=attrid]").val();
					 data['cropwidth']  = form.find("input[name=cropwidth]").val();
					 data['cropheight'] = form.find("input[name=cropheight]").val();
					 data['roportion']  = form.find("input[name=roportion]").val();
					 
					 $.post( frame.UploadConfig.upload_url ,data,function(_RESULT){
						  if(that.windowdialog){that.windowdialog.close();}
						  callBack.call(this,_RESULT);
					 },'json');
		
			  }
	     });
		 
		
	}
}


