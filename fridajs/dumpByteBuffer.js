'use strict';

const moduleName = "GameAssembly.dll";
const constructorRva = ptr("0x1FD18C0");  //ByteBuffer(byte[] buffer, int pos);
const module = Process.findModuleByName(moduleName);

if (module) {
    const targetAddress = module.base.add(constructorRva);
    console.log(`* TargetAddress: ${targetAddress}`);
    
Interceptor.attach(targetAddress, {
        onEnter: function(args) {
            const bufferHandle = args[1];
            const pos = args[2].toInt32();
            
            if (bufferHandle.isNull()) return;

            const length = bufferHandle.add(0x18).readInt();
            const elementsPtr = bufferHandle.add(0x20);
            
            if (length === 123536) {
                console.log(`* ByteBuffer Pos: ${pos}`);
                console.log(`* Buffer Length: ${length}`);
            
                const data = elementsPtr.readByteArray(length);
                File.writeAllBytes(`AcademyFavorScheduleExcelTable.dump`, data)

            }
        }
    });


} else {
    console.error("Hook Failed");
}