var uploadSettings;

/*上传文件基础类*/
var UploadCommon = function(){
	    this.UploadSize = 1; 
		this.acceptDict = [];
		this.tabname = 'upload';
		this.buttonDom={};
		var SelectOptionId = $("#selectoptionId"),wordDom= $("a.word"),that = this, 
		    tabDom = $(".tab  li"),selectmodule=$(".selectmodule"),uploadid=$("#uploadid"),selectoption=$("div.selectoption");
		
		Event();
		
		function Event(){
		    selectedAttachment(wordDom);
			delAttachment(selectoption.find("a.del"));
			tabDom.bind('click',function(){
					 var tabname = $(this).attr('tabname');
					 $(this).addClass('on').siblings().removeClass('on');
					 that.tabname = tabname;
					 if(tabname=='upload'){
						    selectmodule.hide().filter('.upload').show();
					 }else if(tabname=='gallery' ){
							//uploadid.hide();
							selectmodule.hide().filter('.iframegallery').attr('src',parseUrl(SWFUploadConfig.gallery_url,{type:that.filekey})).show();
							//iframeID.attr('src',SWFUploadConfig.gallery_url).show();
					 }else if(tabname=='notuse'){
							selectmodule.hide().filter('.iframenotUse').attr('src',parseUrl(SWFUploadConfig.notuse_url,{type:that.filekey})).show();
							//uploadid.hide();
							//iframeID.attr('src',SWFUploadConfig.notuse_url).show();
					 }
				 
		   });
		}
		
	    /*获取已选择项*/
		function getSelected(){
			 var List = [],iframe ;  
			 //文件库文件可选项
			 if( ((that.UploadSize==1 &&  that.tabname=='gallery') || that.UploadSize>1)  &&  document.getElementById('iframegalleryID')!=null){
			     iframe = $(document.getElementById('iframegalleryID').contentWindow.document);
				 /*iframe中选择的元素[图库|未处理的文件]*/
				 iframe.find("a.selected").each(function(index, element) {
					  List.push({'url':$(this).attr('dataurl'),'appurl':$(this).attr('appurl'),'id':$(this).attr('dataid'),'filename':$(this).attr('filename')});
				 });
			 }
			 
			 //未处理的文件可选项
			 if(((that.UploadSize==1 &&  that.tabname=='notuse') || that.UploadSize>1)  && document.getElementById('iframenotUseID')!=null){
			     iframe = $(document.getElementById('iframenotUseID').contentWindow.document);
				 /*iframe中选择的元素[图库|未处理的文件]*/
				 iframe.find("a.selected").each(function(index, element) {
					  List.push({'url':$(this).attr('dataurl'),'appurl':$(this).attr('appurl'),'id':$(this).attr('dataid'),'filename':$(this).attr('filename')});
				 });
			 }
			 
			 //上传中的可选项
			 if((that.UploadSize==1 &&  that.tabname=='upload') || that.UploadSize>1){
				 $("a.selected").each(function(index, element) {
					  List.push({'url':$(this).attr('dataurl'),'appurl':$(this).attr('appurl'),'id':$(this).attr('dataid'),'filename':$(this).attr('filename')});
				 });
			 }
			 return List;
		}
		
		/*删除附件*/
		function delAttachment(_dom){
			
	    	_dom.bind('click',function(){
				var _that = $(this),_id =_that.attr('dataid'); 
				if(typeof(SWFUploadConfig.delete_url)!=='undefined'){
					 delete_url = SWFUploadConfig.delete_url.replace("#id",_id);
				}else{
				     delete_url = parent.SWFUploadConfig.delete_url.replace("#id",_id);
				}
				
			    $.get(delete_url,function(_data){
					if(_data['status']){
					    _that.parents("li").remove();
					}else{
						alert(_data['msg']);
					}
				},'json');
			});
		}
		
		/*选择附件*/
		function  selectedAttachment(_dom){
			_dom.bind('click',function(){
				 if($(this).is(".selected")){
				 	 $(this).removeClass("selected");
				 }else{
					 if(parent.uploadcommon){
					 	parent.uploadcommon.ParentChekMaxUploadSizecallback($(this));
					 }else{
					 	that.ParentChekMaxUploadSizecallback($(this));
					 }
			     	 /*if(getSelected().length>=that.UploadSize){
					 	 alert('不能超过'+that.UploadSize+"个附件");
					 }else{
						 $(this).addClass("selected");
					 }*/
				 }
			});
		}
		
		function parseUrl(_url,_data){
			return _url.replace(/#([\w]+)/ig,function(_a,_b){ 
			 		if(typeof(_data[_b])=='undefined'){ 
					    return '';
					}
					return _data[_b];
			 });
		}
		
		
		/*判断选择文件选择是否正确*/
		this.checkSelectFileSize = function(_uploadSize){
			   var _List = getSelected();
			   if(_List.length<1 && _uploadSize > this.UploadSize){
			       	return  "只能选择" + this.UploadSize + "条附件";
			   }else if( (_List.length+_uploadSize) >  this.UploadSize ){
				     if( this.UploadSize - _List.length > 0){
				        return "您已选择"+ _List.length +"条附件,还能选择" + (this.UploadSize - _List.length) + "条附件";
					 }else{
					 	return "您已选择"+ _List.length +"条附件,不能在选择";
					 }
			   }else{
			   		return true;
			   }
		}
		
		
		
		
		/*获取*/
		this.ParentChekMaxUploadSizecallback  = function(_dom){
			 var List = getSelected();
			 if(List.length>=that.UploadSize){
			 	alert('不能超过'+that.UploadSize+"个附件");
			 }else{
			 	_dom.addClass("selected");
			 }
			 
		}
		
		
		/*只允许上传的类型*/
		this.setUploadType = function(UploadType){
			this.acceptDict=[];
			if(UploadType!=''){
			      var TypeList  = UploadType.split(",");
				  for(var i=0;i<TypeList.length;i++){
					this.acceptDict.push(TypeList[i]);	
				  }
			}
		}
		
		this.FormatType = function(type,delimiter,_default){
		     var acceptDict=[];
			 if(typeof(_default)=='undefined'){ _default="" };
			 for(var i=0;i<this.acceptDict.length;i++){
			  		acceptDict.push(type+this.acceptDict[i]);
			 }
			 
			 if(acceptDict.length>0) return  acceptDict.join(delimiter);
			 
			 return _default;
			
		}
		
		this.setDom = function(_dom){
	         var uploadsize = $.trim(_dom.attr('uploadsize')),
				 uploadtype = $.trim(_dom.attr('uploadtype')),
				 oldname    = $.trim(_dom.attr('oldname')),
				 filekey    = $.trim(_dom.attr('filekey'));
		     if(uploadsize == 'undefined' || uploadsize==''){ uploadsize = 1;}
			 if(oldname == 'undefined' || oldname==''){ oldname = 0;}
			 this.buttonDom = _dom;
			 this.setUploadType(SWFUploadConfig.uploadType);
			 this.setOldname(oldname);
			 this.setUploadSize(uploadsize);
			 this.setFilekey(filekey);
		};
		
		/*上传最大数量*/
		this.setUploadSize = function(UploadSize){
			this.UploadSize = UploadSize;
		} 
		
		/*上传文件类型标识符KEY*/
		this.setFilekey = function(filekey){
			this.filekey = filekey;
			SWFUploadConfig.upload_url = SWFUploadConfig.upload_url.replace("#type",this.filekey);
		}
		
		/*上传文件是否使用原名*/
		this.setOldname = function(oldname){
			this.oldname = oldname;
			SWFUploadConfig.upload_url = SWFUploadConfig.upload_url.replace("#oldname",this.oldname);
		}
		
		/*上传成功后动作处理*/
		this.UploadUsccess = function(_Lidom,_data){
			var _dom   = $(that.parseTemplate(this.template2,_data));
			SelectOptionId.append(_dom);
			delAttachment(_dom.find("a.del"));
			selectedAttachment(_dom.find("a.word"));
			_Lidom.remove();
		}
		
		/*上传出错处理*/
		this.UploadError = function(_Lidom,_data){
			 _Lidom.addClass('error');
			 _Lidom.find("i.msg").html("哥子出错了:"+_data['message']);
		}
		
		this.createUploadProgressDom = function (_data){
			 var _dom =   $(this.parseTemplate(this.template,_data));
			 _dom.find("a.cancel").bind('click',function(){
			   	      _dom.remove(); 
			 });
			 return _dom;
		}
		
		/*选择后回调*/
		this.callBack = function(fun){
			 var List = getSelected();
			 if(List.length>this.UploadSize){
				  alert('不能超过'+this.UploadSize+"个附件");
			 	return ;
			 }
			 
			 if(fun && typeof(fun) === "function"){
				  
				  this.createAttrId(List);
				  
				  fun.call(this,List);
			 }
		}
		 
		/*设置附件ID*/
		this.createAttrId = function(_list){
	    	 var _inputdom = $('<input type="hidden" class="styleconfig"   name="attachmentid[]" />');
			 var attrList = [];
			 for(var i in _list){
			 	attrList.push(_list[i]['id']);
			 }
			 _inputdom.val(attrList.join(","));
			 if(attrList.length>0){
			 	this.buttonDom.after(_inputdom);
			 }
			 
		};
		
	    /*解析模板
		this.parseTemplate = function (_template,_data){
			 return _template.replace(/\{#(.*?)\}/ig,function(_a,_b){ return _data[_b] || "" });
		}*/
		
		/*编译模板*/
		this.parseTemplate = function(_template,_data){
			return _template.replace(/\{#(.*?)\}/ig,function(_a,_b){ 
			 		if(typeof(_data[_b])=='undefined'){ 
					    return '';
					}
					return _data[_b];
			 });
		}
		
		/*检查是否还有未选择的附件*/
		this.checkNotUse = function(){
			$.get(parseUrl(SWFUploadConfig.notuse_url,{type:that.filekey}),function(_data){
				  if(_data['status']){
					  tabDom.filter('.notuse').show();
				  }else{
				      tabDom.filter('.notuse').hide();
				  }
			},'json');
		}
		
		this.run = function (){ 
		       this.checkNotUse();
		       if (typeof(Worker) !== "undefined") { 
					   //HTML5上传
					   new Html5Upload(this);
			   }else{
				        
						//SWF上传	  
						uploadSettings  = new SWFUpload({
							flash_url : SWFUploadConfig.path+"js/swfupload.swf",
							flash9_url : SWFUploadConfig.path+"swfupload_fp9.swf",
							upload_url: SWFUploadConfig.upload_url,
							post_params: typeof(SWFUploadConfig.post_params) == "undefined"?{}:SWFUploadConfig.post_params,
							file_size_limit : "10MB",
							file_types :that.FormatType("*.",";","*"),
							file_types_description : "All Files",
							file_upload_limit :20,
							file_queue_limit :20,
							custom_settings : {
								progressTarget : SWFUploadConfig.progressTarget,
								cancelButtonId : SWFUploadConfig.cancelButtonId
							},
							debug:false,
							button_image_url: SWFUploadConfig.path+"images/uplode3.gif",
							button_width: "115",
							button_height: "30",
							button_placeholder_id: SWFUploadConfig.button_placeholder_id,
							button_text: '<span class="theFont"></span>',
							button_text_style: ".theFont {font-size: 12; line-height:44px;height:44px;}",
							button_text_left_padding: 0,
							button_text_top_padding: 0,
							swfupload_preload_handler : preLoad,
							swfupload_load_failed_handler : loadFailed,
							file_queued_handler : fileQueued,
							file_queue_error_handler : fileQueueError,
							file_dialog_complete_handler : fileDialogComplete,
							upload_start_handler : uploadStart,
							upload_progress_handler : uploadProgress,
							upload_error_handler : uploadError,
							upload_success_handler : uploadSuccess,
							upload_complete_handler : uploadComplete,
							queue_complete_handler : queueComplete	
						}); 
               }
		
		}
		
		
		
		
		this.template = ['<li id="{#id}">',
							   '<a href="javascript:void(0);" class="cancel" title="取消上传" alt="取消上传"></a>',
							   '<p>{#filename} <i class="msg"></i></p>',
							   '<span><em style="width:0%"></em></span>',
						'</li>'].join("");
						
						
	    this.template2 = ['<li>',
							  '<div class="item">',
								  '<a href="javascript:;" class="word selected" select="selected" filename="{#filename}"  appurl="{#appurl}"  dataurl="{#filepath}" dataid="{#id}">',
									 '<div class="icon"></div>',
									 '<img src="{#appurl}" />',
								  '</a>',
								  '<span><em>{#oldfilename}</em><a href="javascript:void(0);" class="del" dataid="{#id}">删除</a></span>',
							  '</div>',
						  '</li>'].join("");	
	
}

var uploadcommon =  new UploadCommon();


/*HTML5上传工具*/
var  Html5Upload = function(_pthat){
	         var html5uploadbutton = $("#html5uploadbutton"),swfuploadbutton=$("#swfuploadbutton"), fileId = 'multifile';
			 init();
			 function init(){
				  swfuploadbutton.hide();
			 	  html5uploadbutton.show();
				  setFilesAccept();
				  EVENT();
			 }
	        
			 function EVENT(){
					$(document.getElementById(fileId)).bind('change',function(){
						 var FileDom  = document.getElementById(fileId);
						 var msg = _pthat.checkSelectFileSize(FileDom.files.length);
						 if(msg===true){
							 for(var i=0;i<FileDom.files.length;i++){
								  new upload(_pthat,FileDom.files[i]);
							 }
						 }else{
						 	alert(msg);
						 }
						 
					});
			}
			
			
			/*设置文件类型*/
			function setFilesAccept(){
			
			    $(document.getElementById(fileId)).attr('accept',_pthat.FormatType(".",","))
			};
			var  upload = function (_pthat,FileDom){  
			       
				   this.fileProgressWrapper={};
				   var that = this,uploadjingdutiao=$("#uploadjingdutiao");
				   init();
				   function init(){
				        var fd = new FormData(), xhr = new XMLHttpRequest();
						createItem(FileDom);
						fd.append("Filedata",FileDom);
						xhr.upload.addEventListener("progress", uploadProgress, false);//监听上传进度
						xhr.addEventListener("load", uploadComplete, false);
						xhr.addEventListener("error",uploadFailed, false);
						xhr.open("POST",SWFUploadConfig.upload_url);
						xhr.send(fd);
				   
				   }
				   
				   /*创建上传节点*/
				   function createItem(FileDom){
					    var _dom =  _pthat.createUploadProgressDom({"filename":FileDom.name,"id":new Date().getTime()});	 
						uploadjingdutiao.append(_dom);
						that.fileProgressWrapper = _dom;
				   }
				   
				   
				   function uploadProgress(evt) {
						  if (evt.lengthComputable) {
							var percentComplete = Math.round(evt.loaded * 100 / evt.total);
							that.fileProgressWrapper.find("em").width(percentComplete+"%").html('');
						  }
						  else {
							 //document.getElementById('progressNumber').innerHTML = 'unable to compute';
						  }
				   }
					
				   function uploadComplete(evt) {
					     var serverData = evt.target.responseText ;
						 if ("string" == typeof serverData){
							   serverData  =  $.parseJSON(evt.target.responseText);
						 }
						 
						 if(serverData['status']){
					        _pthat.UploadUsccess(that.fileProgressWrapper,serverData);
						 }else{
						    _pthat.UploadError(that.fileProgressWrapper,serverData);
						 }
				   }
					
				   function uploadFailed(evt) {
					  alert("There was an error attempting to upload the file.");
				   }
					
				   function uploadCanceled(evt) {
					  alert("The upload has been canceled by the user or the browser dropped the connection.");
				   }
			}
				
}



/*swfupload上传工具*/
function FileProgress(file, targetID) {
	
	this.fileProgressID = file.id;
    this.ID = targetID;
	this.opacity = 100;
	this.height = 0;
	
    this.fileProgressWrapper = document.getElementById(this.fileProgressID);
	if (!this.fileProgressWrapper) {
		 var _data={"filename":file.name,"id":this.fileProgressID},
		     //template = this.template();
		 //template = template.replace(/\{#(.*?)\}/ig,function(_a,_b){ return _data[_b] || "" });
		 template = uploadcommon.createUploadProgressDom(_data);
		 this.fileProgressWrapper = $(template);
		 $("#"+targetID).append(this.fileProgressWrapper);
		
	}else{
		this.fileProgressWrapper = $(this.fileProgressWrapper);
	
		
	}
}
 


//上传进度
FileProgress.prototype.setProgress = function (percentage) {
	if(percentage<100){
		this.fileProgressWrapper.find("em").width(percentage+"%").html('');
	}
};
//上传完成
FileProgress.prototype.setComplete = function(serverData) {
	var  that=this;
	
	if ("string" == typeof serverData){
	     serverData  =  $.parseJSON(serverData);
	}
	
	
	
	this.fileProgressWrapper.find("em").width("100%").html("");
	
	if(serverData['status']){
		 uploadcommon.UploadUsccess(this.fileProgressWrapper,serverData)
	}else{
		uploadcommon.UploadError(that.fileProgressWrapper,serverData);
	}
	
};
//上传出错
FileProgress.prototype.setError = function () {
   this.fileProgressWrapper.addClass('uploaderror');
   
};

//取消上传状态
FileProgress.prototype.setCancelled = function () {
	this.fileProgressWrapper.remove();
	
};

//更改上传状态
FileProgress.prototype.setStatus = function (status) {
	//this.fileProgressWrapper.find(".uploadstatus").html(status);
};

//取消上传
FileProgress.prototype.toggleCancel = function (show, swfUploadInstance) {
	 if (swfUploadInstance) {
		var fileID = this.fileProgressID;
		this.fileProgressWrapper.find("a.cancel").bind('click',function () {
			  swfUploadInstance.cancelUpload(fileID);
			return false;
		});
	}
};

FileProgress.prototype.message=function(_val){
	 
}

//取消上传后的状态
FileProgress.prototype.disappear = function () {};




 

		