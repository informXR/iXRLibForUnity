function ProcessInteractionType(enumValue) {
    if (enumValue === 1) return iXRLib.InteractionType.eBool;
    if (enumValue === 2) return iXRLib.InteractionType.eSelect;
    if (enumValue === 3) return iXRLib.InteractionType.eText;
    if (enumValue === 4) return iXRLib.InteractionType.eRating;
    if (enumValue === 5) return iXRLib.InteractionType.eNumber;
    return iXRLib.InteractionType.eNull;
}

function ProcessResultOption(enumValue) {
    if (enumValue === 1) return iXRLib.ResultOptions.ePass;
    if (enumValue === 2) return iXRLib.ResultOptions.eFail;
    if (enumValue === 3) return iXRLib.ResultOptions.eComplete;
    if (enumValue === 4) return iXRLib.ResultOptions.eIncomplete;
    return iXRLib.ResultOptions.eNull;
}

function ProcessDictString(dictPtr) {
    const dictString = UTF8ToString(dictPtr);
    const dictMeta = new iXRLib.iXRDictStrings();
    if (!dictString) return dictMeta;
    const parsed = JSON.parse(dictString);
    for (let key in parsed) {
        if (parsed.hasOwnProperty(key)) {
            dictMeta.set(key, parsed[key]);
        }
    }
    return dictMeta;
}