var currentUrl = window.location.href;

if (currentUrl.indexOf('/complete-lesson?id') > -1 || currentUrl.toLowerCase().indexOf('/wallet') > -1) {
	CKEDITOR.editorConfig = function (config) {
		config.toolbarGroups = [
			{ name: 'clipboard', groups: ['clipboard', 'undo'] },
			{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
			{ name: 'forms', groups: ['forms'] },
			{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
			{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph','scayt'] },
			{ name: 'links', groups: ['links'] },
			{ name: 'insert', groups: ['insert'] },
			'/',
			{ name: 'styles', groups: ['styles'] },
			{ name: 'colors', groups: ['colors'] },
			{ name: 'tools', groups: ['tools'] },
			{ name: 'others', groups: ['others'] },
			{ name: 'about', groups: ['about'] }
		];

		config.removeButtons = 'Source,Save,Templates,NewPage,Preview,Print,Undo,Redo,Cut,Copy,Paste,PasteText,PasteFromWord,Find,Replace,SelectAll,,Form,Checkbox,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,Underline,Subscript,Superscript,CopyFormatting,RemoveFormat,Blockquote,CreateDiv,JustifyLeft,JustifyCenter,JustifyRight,JustifyBlock,BidiLtr,BidiRtl,Language,Anchor,Flash,HorizontalRule,Smiley,SpecialChar,PageBreak,Iframe,Styles,Format,Font,FontSize,TextColor,BGColor,ShowBlocks,Maximize,About';
	};
}
else {
	CKEDITOR.editorConfig = function (config) {
		config.toolbarGroups = [
			{ name: 'mode', groups: ['mode'] },
			{ name: 'clipboard', groups: ['clipboard', 'undo'] },
			{ name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
			{ name: 'links' },
			{ name: 'insert' },
			'/',
			{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
			{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align'] },
			{ name: 'styles' },
			{ name: 'colors' },
			{ name: 'alignment', groups: ['left', 'right'] },
		];
		config.extraPlugins = 'imageResize';
		config.removeButtons = 'Underline,Subscript,Superscript';

		config.format_tags = 'p;h1;h2;h3;pre';

		config.removeDialogTabs = 'image:advanced;link:advanced';

		

		config.imageResize = { maxWidth: 800, maxHeight: 800 };


		//CKEDITOR.config.imageResize.maxWidth = 800;
		//CKEDITOR.config.imageResize.maxHeight = 800;

	};
}


