/*
BlueArchive TableBundles bytes File dump

Usage:
change targetName to what you want to decrypt
(the first 256 bytes will show on console, full decrypted data will dump to targetName.bin)

*/



'use strict'

const targetName = "AcademyMessanger2ExcelTable";
const functionRVA = ptr(0x117EF60);
const module = Process.getModuleByName("GameAssembly.dll");

if(module){
    const baseAddress = module.base;
    const targetAddress = baseAddress.add(functionRVA);

    console.log(`* TargetAddress: ${targetAddress}`);


Interceptor.attach(targetAddress, {
    onEnter: function(args){
        const nameHandle = args[0];
        const bytesHandle = args[1];

        let NameLength = nameHandle.add(0x10).readInt();
        let name = nameHandle.add(0x14).readUtf16String(NameLength);

        // console.log(`* Dumped Name: ${name}`);
        this.targetHit = (name == targetName);
        this.bytesPtr = bytesHandle;
    },

    onLeave: function(retval){
        if(this.targetHit){
            const arrayLength = this.bytesPtr.add(0x18).readInt();
            const dataptr = this.bytesPtr.add(0x20);

            if(arrayLength > 0){
                console.log(`TargetName: ${targetName}`);

                console.log(hexdump(dataptr, {
                    offset: 0,
                    length: Math.min(arrayLength, 256),
                    header: true,
                    ansi: true
                }));

                const fulldata = dataptr.readByteArray(arrayLength);
                File.writeAllBytes(`${targetName}.bin`, fulldata);
            }
    }
}
})
}else{
    console.error("Hook Failed");
}
