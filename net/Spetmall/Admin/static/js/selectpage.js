var selectPage = function(){
	
	var $this = this;
		this.reloadStatus = false;
		this.msg          = "ss";
		this.config       = { page:1 ,pagesize:20 , data: {} , scroll:$(window) , content:"" , type : 'GET' , dataType:"json" };
		
	this.setUrl = function(_url){
		this.config.url = _url;
		return this;
	}
	
	this.init = function(_config){
		for(var J in _config){
			this.config[J] = _config[J];
		}
		this.scroll();
		return this;
	}
   
    this.setPage = function(_page){
		this.config.page = _page;
		return this;
	}
	
	this.setPagesize = function(_pagesize){
		this.config.pagesize = _pagesize;
		return this;
	}
	
	
	this.setData = function(_data){
		this.config.data = _data;
		return this;
	}
	
	this.bind = function(_dom){
			 
	}
	
	//创建内容
	this.appendContent=function(_data){
		
		 var _str_content=_data.content,_$dom_content = $(_str_content);
		 $this.bind(_$dom_content);
		 $this.config.content.append(_$dom_content);
	}
	this.scroll = function(){
	    this.config.scroll.scroll(function(){
			  var _containerheight=$this.config.content.outerHeight(),_height = $(this).outerHeight(),_scrollTop=$(this).scrollTop();     
			  if(_height+_scrollTop+10 >= _containerheight && !$this.reloadStatus){
			  		$this.request(function(_data){
						 _data.datacount<=1 && $this.afterAll(_data);
					});
			  }
		 });
		 
	}
	
	//请求前
	this.requestBefore = function(){
		$this.reloadStatus = true;
		yunmessage.loading();
	}
	
	//请求后
	this.requestAfter = function(){
		if(yunmessage.loadingdom) yunmessage.loadingdom.close();
	}
	
	//到低了
	this.afterAll = function(){
	    
	}
	
	//请求成功之前
	this.successBefore = function(_data){
		
	}
	
	//没有数据
	this.empty  = function(_data){
		
	}
	
	this.paginate = function(){
	     
		 this.request(function(_data){
		 		_data.datacount < 1 &&  $this.empty(_data,$this.config.data);	
		 });
	}
	
	this.request = function(_callback){
		this.requestBefore();
		var _query  =  ($this.config.url.indexOf("?")>-1?"&":"?")+"_t="+new Date().getTime()+"&pagesize="+ ($this.config.pagesize) +"&page="+ ($this.config.page) +"&"+($this.config.dataType=='json'?"isAjax=1":"");
		var _url    = $this.config.url+_query;
		$.ajax({
			type : $this.config.type,
			url  : _url,
			dataType: $this.config.dataType,
			data: $this.config.data,
			complete:function(_data){
			    $this.requestAfter();
			},
			success : function(_data) {
				 
				 $this.successBefore(_data);
				 
				 $this.appendContent(_data);
				 
				 _callback &&  _callback(_data);
				 
				 if( _data.datacount > 0 ){
					  $this.config.page+=1; 
					  if( $this.config.scroll.scrollTop()<=0){
					  	  $this.config.scroll.scrollTop(1);
						  if($this.config.scroll.scrollTop()>0) 
						     $this.config.scroll.scrollTop(0);
						  else
						     $this.request();
					  }
					  $this.reloadStatus = false;
					  
				 }else{
					  //$this.empty(_data);
					  $this.reloadStatus = true; 
				 }
			},
			error:function(data, textStatus){
				if(textStatus!='200'){
					yunmessage.msg("请求错误");
				}else{
					yunmessage.msg("请求错误");
				}
				
			}
		});	
	}
		
}