import { useEffect, useMemo, useRef, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { EbayButton } from '@ebay/ui-core-react/ebay-button';
import { EbayFilePreviewCard } from '@ebay/ui-core-react/ebay-file-preview-card';
import { ReactComponent as AddImageIcon } from '@ebay/skin/dist/svg/icon/icon-add-image-24.svg';
import FileService from '../../../services/FileService';
import Notice from '../../../components/Common/CustomNotification';

const STATIC_ATTRIBUTE_LIBRARY = {
    'Fabric Type': [
        'Canvas',
        'Chambray',
        'Chiffon',
        'Corduroy',
        'Crochet',
        'Damask',
        'Denim',
        'Down',
        'Flannel',
        'Fleece',
        'Jersey',
        'Knit',
        'Lace',
        'Microfiber',
        'Rayon',
        'Satin',
        'Tweed',
        'Twill',
        'Velvet',
        'Woven'
    ],
    Accents: [
        'Beaded',
        'Bow',
        'Buckle',
        'Button',
        'Crochet',
        'Embroidered',
        'Fringe',
        'Fur Trim',
        'Glitter',
        'Logo',
        'Pleated',
        'Quilted',
        'Rhinestone',
        'Ruffle',
        'Sequin',
        'Strap',
        'Studded',
        'Tasseled',
        'Zipper'
    ]
};

const buildCombinationKey = (values) => {
    return Object.entries(values)
        .sort(([a], [b]) => a.localeCompare(b))
        .map(([attribute, option]) => `${attribute}:${option}`)
        .join('|');
};

const generateCombinations = (attributes, selectedOptions) => {
    const activeAttributes = attributes.filter((attr) => (selectedOptions[attr.name] || []).length > 0);

    if (!activeAttributes.length) {
        return [];
    }

    const seed = [{}];

    const combinations = activeAttributes.reduce((acc, attribute) => {
        const options = selectedOptions[attribute.name] || [];

        if (!options.length) {
            return acc;
        }

        const next = [];
        acc.forEach((combo) => {
            options.forEach((option) => {
                next.push({ ...combo, [attribute.name]: option });
            });
        });

        return next;
    }, seed);

    return combinations.map((values) => ({
        key: buildCombinationKey(values),
        attributeValues: values
    }));
};

const CollapsibleCard = ({ title, helper, countBadge, children, defaultOpen = true }) => {
    const [open, setOpen] = useState(defaultOpen);

    return (
        <div className="border border-gray-200 rounded-md">
            <button
                type="button"
                onClick={() => setOpen((prev) => !prev)}
                className="w-full flex items-center justify-between px-4 py-3 text-left hover:bg-gray-50"
            >
                <div>
                    <p className="text-sm font-medium text-gray-800 flex items-center gap-2">
                        {title}
                        {typeof countBadge === 'number' && (
                            <span className="text-xs font-semibold text-gray-500 bg-gray-100 px-2 py-0.5 rounded-full">
                                {countBadge}
                            </span>
                        )}
                    </p>
                    {helper && <p className="text-xs text-gray-500 mt-0.5">{helper}</p>}
                </div>
                <span className="text-gray-500 text-lg leading-none">{open ? '▾' : '▸'}</span>
            </button>
            {open && <div className="border-t border-gray-200 px-4 py-4 bg-white">{children}</div>}
        </div>
    );
};

const BulkActionButton = ({ onClick, children }) => (
    <button
        type="button"
        onClick={onClick}
        className="inline-flex items-center justify-center rounded border border-gray-300 bg-white px-3 py-2 text-xs font-medium text-gray-700 hover:bg-gray-50"
    >
        {children}
    </button>
);

const MAX_DEFAULT_PHOTOS = 24;
const createPhotoId = () => `photo-${Date.now()}-${Math.random().toString(36).slice(2, 10)}`;

const normalizePhoto = (item) => {
    if (!item) {
        return null;
    }

    return {
        id: item.id ?? createPhotoId(),
        name: item.name ?? 'Photo',
        type: item.type ?? 'image',
        size: item.size,
        src: item.src ?? '',
        preview: item.preview && item.preview.startsWith('blob:') ? item.preview : null,
        uploading: Boolean(item.uploading && !item.src)
    };
};

const stripPreview = (item) => {
    if (!item) {
        return null;
    }

    const { preview, ...rest } = item;
    return rest;
};

const VariationForm = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const fromListing = Boolean(location.state?.fromListing);
    const initialVariationData = location.state?.variationData ?? null;
    const categoryDetail = location.state?.categoryDetail ?? null;
    const initialSpecificSelections = location.state?.selectedSpecifics ?? null;
    const listingDraft = location.state?.listingDraft ?? null;
    const listingId = listingDraft?.listingId ?? location.state?.listingId ?? null;

    const attributePanelRef = useRef(null);
    const defaultPhotosInputRef = useRef(null);
    const variationRowPhotoInputRef = useRef(null);
    const shouldPreserveMediaRef = useRef(false);

    const variationSpecifics = useMemo(() => {
        if (!categoryDetail?.specifics) {
            return [];
        }

        return categoryDetail.specifics
            .filter((specific) => Array.isArray(specific.values) && specific.values.length > 0)
            .slice(0, 5);
    }, [categoryDetail]);

    const buildInitialAttributes = () => {
        if (initialVariationData?.attributes?.length) {
            const cloned = initialVariationData.attributes.map((attr) => ({ ...attr }));
            if (cloned.length && !cloned.some((attr) => attr.selected)) {
                cloned[0].selected = true;
            }
            return cloned;
        }

        if (variationSpecifics.length) {
            return variationSpecifics.map((specific, index) => ({
                id: specific.id,
                name: specific.name,
                selected: index === 0
            }));
        }

        return [
            { id: 'attr-fabric-type', name: 'Fabric Type', selected: true },
            { id: 'attr-accents', name: 'Accents', selected: false }
        ];
    };

    const buildInitialSelectedOptions = () => {
        if (initialVariationData?.selectedOptions) {
            return Object.entries(initialVariationData.selectedOptions).reduce((acc, [name, options]) => {
                acc[name] = Array.isArray(options) ? [...options] : [];
                return acc;
            }, {});
        }

        if (!initialSpecificSelections) {
            return {};
        }

        const preset = {};
        variationSpecifics.forEach((specific) => {
            const selection = initialSpecificSelections?.[specific.id];
            if (!selection) {
                return;
            }

            const values = Array.isArray(selection) ? selection : [selection];
            const unique = Array.from(new Set(values.filter(Boolean)));
            if (unique.length) {
                preset[specific.name] = unique;
            }
        });

        return preset;
    };

    const buildInitialVariationRows = () => {
        if (!initialVariationData?.variationRows) {
            return [];
        }

        return initialVariationData.variationRows.map((row) => ({
            ...row,
            attributeValues: { ...(row.attributeValues || {}) },
            photo: normalizePhoto(row.photo)
        }));
    };

    const buildInitialDefaultPhotos = () => {
        if (!initialVariationData?.defaultPhotos) {
            return [];
        }

        return initialVariationData.defaultPhotos
            .map((item) => normalizePhoto(item))
            .filter(Boolean);
    };

    const buildInitialExcludedKeys = () => {
        if (!initialVariationData?.excludedKeys) {
            return [];
        }

        return [...initialVariationData.excludedKeys];
    };

    const resolveInitialMode = () => {
        if (!initialVariationData) {
            return 'edit';
        }

        const hasVariationRows = Array.isArray(initialVariationData.variationRows) && initialVariationData.variationRows.length > 0;
        const hasSelectedOptions = initialVariationData.selectedOptions
            ? Object.values(initialVariationData.selectedOptions).some(
                  (options) => Array.isArray(options) && options.length > 0
              )
            : false;

        if (!hasVariationRows || !hasSelectedOptions) {
            return 'edit';
        }

        return initialVariationData.mode ?? 'summary';
    };

    const [mode, setMode] = useState(resolveInitialMode);
    const [editSnapshot, setEditSnapshot] = useState(null);
    const [attributes, setAttributes] = useState(buildInitialAttributes);
    const [selectedOptions, setSelectedOptions] = useState(buildInitialSelectedOptions);
    const [showAddAttributeInput, setShowAddAttributeInput] = useState(false);
    const [newAttributeName, setNewAttributeName] = useState('');
    const [showCustomOptionInput, setShowCustomOptionInput] = useState(false);
    const [customOptionValue, setCustomOptionValue] = useState('');
    const [variationRows, setVariationRows] = useState(buildInitialVariationRows);
    const [selectedRowKeys, setSelectedRowKeys] = useState([]);
    const [excludedKeys, setExcludedKeys] = useState(buildInitialExcludedKeys);
    const [defaultPhotos, setDefaultPhotos] = useState(buildInitialDefaultPhotos);
    const [activePhotoRowKey, setActivePhotoRowKey] = useState(null);

    const attributeLibrary = useMemo(() => {
        const library = { ...STATIC_ATTRIBUTE_LIBRARY };

        variationSpecifics.forEach((specific) => {
            if (Array.isArray(specific.values) && specific.values.length) {
                library[specific.name] = specific.values;
            }
        });

        Object.entries(selectedOptions).forEach(([name, options]) => {
            if (!Array.isArray(options)) {
                return;
            }

            library[name] = Array.from(new Set([...(library[name] || []), ...options]));
        });

        return library;
    }, [variationSpecifics, selectedOptions]);

    const combinations = useMemo(
        () => generateCombinations(attributes, selectedOptions),
        [attributes, selectedOptions]
    );

    const defaultPhotosCountLabel = `${defaultPhotos.length}/${MAX_DEFAULT_PHOTOS}`;

    const defaultPhotosRef = useRef(defaultPhotos);
    const variationRowsRef = useRef(variationRows);

    useEffect(() => {
        defaultPhotosRef.current = defaultPhotos;
    }, [defaultPhotos]);

    useEffect(() => {
        variationRowsRef.current = variationRows;
    }, [variationRows]);

    useEffect(() => {
        return () => {
            if (shouldPreserveMediaRef.current) {
                return;
            }

            defaultPhotosRef.current.forEach((item) => {
                const previewUrl = item?.preview ?? item?.src;
                if (previewUrl && previewUrl.startsWith('blob:')) {
                    URL.revokeObjectURL(previewUrl);
                }
            });

            variationRowsRef.current.forEach((row) => {
                const previewUrl = row?.photo?.preview ?? row?.photo?.src;
                if (previewUrl && previewUrl.startsWith('blob:')) {
                    URL.revokeObjectURL(previewUrl);
                }
            });
        };
    }, []);

    useEffect(() => {
        setExcludedKeys((prev) => {
            const validKeys = combinations.map((combo) => combo.key);
            const filtered = prev.filter((key) => validKeys.includes(key));
            return filtered.length === prev.length ? prev : filtered;
        });
    }, [combinations]);

    useEffect(() => {
        setVariationRows((prevRows) => {
            const manualRows = prevRows.filter((row) => row.isManual);
            const autoRows = prevRows.filter((row) => !row.isManual);
            const autoMap = new Map(autoRows.map((row) => [row.key, row]));

            const nextAutoRows = combinations
                .filter((combo) => !excludedKeys.includes(combo.key))
                .map((combo) => {
                    const existing = autoMap.get(combo.key);
                    if (existing) {
                        return { ...existing, attributeValues: combo.attributeValues };
                    }

                    return {
                        key: combo.key,
                        attributeValues: combo.attributeValues,
                        price: '',
                        quantity: '',
                        sku: '',
                        upc: '',
                        photo: null,
                        isManual: false
                    };
                });

            return [...nextAutoRows, ...manualRows];
        });
    }, [combinations, excludedKeys]);

    useEffect(() => {
        setSelectedRowKeys((prev) => prev.filter((key) => variationRows.some((row) => row.key === key)));
    }, [variationRows]);

    const buildVariationPayload = () => {
        const normalizedAttributes = attributes.map((attr, index) => ({
            ...attr,
            selected: attr.selected ?? index === 0
        }));

        if (normalizedAttributes.length && !normalizedAttributes.some((attr) => attr.selected)) {
            normalizedAttributes[0].selected = true;
        }

        const normalizedSelectedOptions = Object.entries(selectedOptions).reduce((acc, [name, options]) => {
            if (Array.isArray(options) && options.length) {
                acc[name] = Array.from(new Set(options.filter(Boolean)));
            }
            return acc;
        }, {});

        const normalizedRows = variationRows.map((row) => ({
            ...row,
            attributeValues: { ...(row.attributeValues || {}) },
            photo: stripPreview(row.photo)
        }));

        const normalizedDefaultPhotos = defaultPhotos
            .map((item) => stripPreview(item))
            .filter(Boolean);

        const hasVariationRows = normalizedRows.length > 0;
        const hasSelectedOptions = Object.keys(normalizedSelectedOptions).length > 0;
        const nextMode = hasVariationRows && hasSelectedOptions ? 'summary' : 'edit';

        return {
            attributes: normalizedAttributes,
            selectedOptions: normalizedSelectedOptions,
            variationRows: normalizedRows,
            excludedKeys: [...excludedKeys],
            defaultPhotos: normalizedDefaultPhotos,
            categoryId: categoryDetail?.id ?? null,
            categoryName: categoryDetail?.name ?? null,
            mode: nextMode
        };
    };

    const handleSaveAndReturn = (extraState = {}) => {
        shouldPreserveMediaRef.current = true;
        const payload = buildVariationPayload();

        const sharedState = {
            variationData: payload,
            ...(listingDraft ? { listingDraft } : {}),
            ...(listingId ? { listingId } : {}),
            ...extraState
        };

        const targetPath = listingId ? `/listing-form/${listingId}` : '/listing-form';

        if (fromListing) {
            navigate(targetPath, {
                state: sharedState,
                replace: true
            });
        } else {
            navigate(targetPath, {
                state: sharedState
            });
        }
    };

    const handleSaveAndClose = () => {
        handleSaveAndReturn();
    };

    const handleSaveAndPreview = () => {
        handleSaveAndReturn({ preview: true });
    };

    const handleBackNavigation = () => {
        if (fromListing) {
            const nextState = {
                ...(listingDraft ? { listingDraft } : {}),
                ...(initialVariationData ? { variationData: initialVariationData } : {}),
                ...(listingId ? { listingId } : {})
            };

            const targetPath = listingId ? `/listing-form/${listingId}` : '/listing-form';

            navigate(targetPath, {
                replace: true,
                state: Object.keys(nextState).length ? nextState : undefined
            });
        } else {
            navigate(-1);
        }
    };

    const selectAttribute = (attrId) => {
        setAttributes((prev) =>
            prev.map((attr) => ({
                ...attr,
                selected: attr.id === attrId
            }))
        );
    };

    const removeAttribute = (attrName) => {
        setAttributes((prev) => {
            const filtered = prev.filter((attr) => attr.name !== attrName);
            if (!filtered.length) {
                return [];
            }

            const hasSelected = filtered.some((attr) => attr.selected);
            return filtered.map((attr, index) => ({
                ...attr,
                selected: hasSelected ? attr.selected : index === 0
            }));
        });

        setSelectedOptions((prev) => {
            const next = { ...prev };
            delete next[attrName];
            return next;
        });

        setExcludedKeys((prev) => prev.filter((key) => !key.includes(`${attrName}:`)));
    };

    const toggleOption = (attribute, option) => {
        setSelectedOptions((prev) => {
            const current = prev[attribute] || [];
            const updated = current.includes(option)
                ? current.filter((item) => item !== option)
                : [...current, option];
            return { ...prev, [attribute]: updated };
        });
        setExcludedKeys((prev) => prev.filter((key) => !key.includes(`${attribute}:${option}`)));
    };

    const removeOption = (attribute, option) => {
        toggleOption(attribute, option);
    };

    const currentAttribute = attributes.find((attr) => attr.selected);
    const currentOptions = currentAttribute ? selectedOptions[currentAttribute.name] || [] : [];
    const suggestionOptions = currentAttribute ? attributeLibrary[currentAttribute.name] || [] : [];

    const allSelectedCount = variationRows.length;

    const handleAddAttribute = () => {
        const trimmed = newAttributeName.trim();
        if (!trimmed) {
            return;
        }

        const exists = attributes.some((attr) => attr.name.toLowerCase() === trimmed.toLowerCase());
        if (exists) {
            setNewAttributeName('');
            setShowAddAttributeInput(false);
            return;
        }

        setAttributes((prev) => [
            ...prev.map((attr) => ({ ...attr, selected: false })),
            { id: `attr-${Date.now()}`, name: trimmed, selected: true }
        ]);

        setSelectedOptions((prev) => ({
            ...prev,
            [trimmed]: []
        }));

        setNewAttributeName('');
        setShowAddAttributeInput(false);
    };

    const handleAddCustomOption = () => {
        if (!currentAttribute || !customOptionValue.trim()) {
            return;
        }

        const value = customOptionValue.trim();
        setSelectedOptions((prev) => {
            const existing = prev[currentAttribute.name] || [];
            if (existing.includes(value)) {
                return prev;
            }
            return {
                ...prev,
                [currentAttribute.name]: [...existing, value]
            };
        });

        setCustomOptionValue('');
        setShowCustomOptionInput(false);
    };

    const toggleRowSelection = (key) => {
        setSelectedRowKeys((prev) => {
            if (prev.includes(key)) {
                return prev.filter((item) => item !== key);
            }
            return [...prev, key];
        });
    };

    const areAllRowsSelected = variationRows.length > 0 && selectedRowKeys.length === variationRows.length;

    const toggleSelectAllRows = () => {
        if (areAllRowsSelected) {
            setSelectedRowKeys([]);
        } else {
            setSelectedRowKeys(variationRows.map((row) => row.key));
        }
    };

    const updateRowField = (key, field, value) => {
        setVariationRows((prev) => prev.map((row) => (row.key === key ? { ...row, [field]: value } : row)));
    };

    const handleBulkUpdate = (field, label) => {
        const targetKeys = selectedRowKeys.length ? selectedRowKeys : variationRows.map((row) => row.key);
        if (!targetKeys.length) {
            return;
        }

        const input = window.prompt(`Enter ${label}`);
        if (input === null) {
            return;
        }

        setVariationRows((prev) =>
            prev.map((row) => (targetKeys.includes(row.key) ? { ...row, [field]: input } : row))
        );
    };

    const handleDeleteRows = (keys) => {
        if (!keys.length) {
            return;
        }

        const rowsToDelete = variationRows.filter((row) => keys.includes(row.key));

        rowsToDelete.forEach((row) => {
            const previewUrl = row.photo?.preview ?? row.photo?.src;
            if (previewUrl && previewUrl.startsWith('blob:')) {
                URL.revokeObjectURL(previewUrl);
            }
        });

        setVariationRows((prev) => prev.filter((row) => !(row.isManual && keys.includes(row.key))));

        setExcludedKeys((prev) => {
            const next = [...prev];
            rowsToDelete
                .filter((row) => !row.isManual)
                .forEach((row) => {
                    if (!next.includes(row.key)) {
                        next.push(row.key);
                    }
                });
            return next;
        });

        setSelectedRowKeys((prev) => prev.filter((key) => !keys.includes(key)));
    };

    const handleAddCombination = () => {
        const attributeValues = attributes.reduce((acc, attribute) => {
            const options = selectedOptions[attribute.name] || [];
            if (options.length) {
                acc[attribute.name] = options[0];
            }
            return acc;
        }, {});

        const key = `manual-${Date.now()}`;

        setVariationRows((prev) => [
            ...prev,
            {
                key,
                attributeValues,
                price: '',
                quantity: '',
                sku: '',
                upc: '',
                photo: null,
                isManual: true
            }
        ]);
    };

    const handleEditAttributes = () => {
        setEditSnapshot({
            attributes: attributes.map((attr) => ({ ...attr })),
            selectedOptions: JSON.parse(JSON.stringify(selectedOptions)),
            excludedKeys: [...excludedKeys],
            variationRows: JSON.parse(JSON.stringify(variationRows))
        });
        setMode('edit');
        setShowAddAttributeInput(false);
        setShowCustomOptionInput(false);
        setNewAttributeName('');
        setCustomOptionValue('');

        if (attributePanelRef.current) {
            attributePanelRef.current.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    };

    const createPhotoEntry = (file) => {
        const previewUrl = URL.createObjectURL(file);
        return {
            id: createPhotoId(),
            name: file.name,
            type: file.type.startsWith('video/') ? 'video' : 'image',
            size: file.size,
            src: previewUrl,
            preview: previewUrl,
            uploading: true
        };
    };

    const uploadDefaultPhoto = async (entryId, file) => {
        try {
            const response = await FileService.upload(file);
            const uploadedUrl = response?.data;

            if (!uploadedUrl || typeof uploadedUrl !== 'string') {
                throw new Error('Missing uploaded photo url');
            }

            setDefaultPhotos((prev) =>
                prev.map((photo) => {
                    if (photo.id !== entryId) {
                        return photo;
                    }

                    if (photo.preview && photo.preview.startsWith('blob:')) {
                        URL.revokeObjectURL(photo.preview);
                    }

                    return {
                        ...photo,
                        src: uploadedUrl,
                        preview: null,
                        uploading: false
                    };
                })
            );
        } catch (error) {
            console.error('Default photo upload failed', error);
            Notice({
                msg: `Failed to upload ${file.name}.`,
                desc: 'Please try again.',
                isSuccess: false
            });

            setDefaultPhotos((prev) =>
                prev.filter((photo) => {
                    if (photo.id !== entryId) {
                        return true;
                    }

                    if (photo.preview && photo.preview.startsWith('blob:')) {
                        URL.revokeObjectURL(photo.preview);
                    }

                    return false;
                })
            );
        }
    };

    const uploadVariationRowPhoto = async (rowKey, photoId, file) => {
        try {
            const response = await FileService.upload(file);
            const uploadedUrl = response?.data;

            if (!uploadedUrl || typeof uploadedUrl !== 'string') {
                throw new Error('Missing uploaded variation photo url');
            }

            setVariationRows((prev) =>
                prev.map((row) => {
                    if (row.key !== rowKey || !row.photo || row.photo.id !== photoId) {
                        return row;
                    }

                    if (row.photo.preview && row.photo.preview.startsWith('blob:')) {
                        URL.revokeObjectURL(row.photo.preview);
                    }

                    return {
                        ...row,
                        photo: {
                            ...row.photo,
                            src: uploadedUrl,
                            preview: null,
                            uploading: false
                        }
                    };
                })
            );
        } catch (error) {
            console.error('Variation photo upload failed', error);
            Notice({
                msg: `Failed to upload ${file.name}.`,
                desc: 'Please try again.',
                isSuccess: false
            });

            setVariationRows((prev) =>
                prev.map((row) => {
                    if (row.key !== rowKey) {
                        return row;
                    }

                    if (row.photo?.preview && row.photo.preview.startsWith('blob:')) {
                        URL.revokeObjectURL(row.photo.preview);
                    }

                    return { ...row, photo: null };
                })
            );
        }
    };

    const handleDefaultPhotosSelect = (files) => {
        if (!files?.length) {
            return;
        }

        const validFiles = Array.from(files).filter(
            (file) => file.type.startsWith('image/') || file.type.startsWith('video/')
        );

        if (!validFiles.length) {
            Notice({ msg: 'Only image or video files can be added.', isSuccess: false });
            return;
        }

        const available = Math.max(0, MAX_DEFAULT_PHOTOS - defaultPhotos.length);
        if (available <= 0) {
            Notice({ msg: 'You have reached the maximum number of default photos.', isSuccess: false });
            return;
        }

        const limited = validFiles.slice(0, available);
        const prepared = limited.map((file) => createPhotoEntry(file));

        setDefaultPhotos((prev) => [...prev, ...prepared]);

        prepared.forEach((entry, index) => {
            const originalFile = limited[index];
            uploadDefaultPhoto(entry.id, originalFile);
        });

        if (validFiles.length > limited.length) {
            Notice({
                msg: `Only ${available} file(s) were added due to the limit.`,
                isSuccess: false
            });
        }
    };

    const handleDefaultPhotosInput = (event) => {
        handleDefaultPhotosSelect(event.target.files);
        event.target.value = '';
    };

    const openDefaultPhotosPicker = () => {
        if (defaultPhotos.length >= MAX_DEFAULT_PHOTOS) {
            return;
        }
        defaultPhotosInputRef.current?.click();
    };

    const removeDefaultPhoto = (id) => {
        setDefaultPhotos((prev) => {
            const target = prev.find((item) => item.id === id);
            const previewUrl = target?.preview ?? target?.src;
            if (previewUrl && previewUrl.startsWith('blob:')) {
                URL.revokeObjectURL(previewUrl);
            }

            return prev.filter((item) => item.id !== id);
        });
    };

    const openVariationRowPhotoPicker = (rowKey) => {
        setActivePhotoRowKey(rowKey);
        variationRowPhotoInputRef.current?.click();
    };

    const handleVariationRowPhotoInput = (event) => {
        const file = event.target.files?.[0];
        event.target.value = '';
        const targetRowKey = activePhotoRowKey;
        setActivePhotoRowKey(null);

        if (!file || !file.type.startsWith('image/') || !targetRowKey) {
            return;
        }

        const entry = createPhotoEntry(file);

        setVariationRows((prev) =>
            prev.map((row) => {
                if (row.key !== targetRowKey) {
                    return row;
                }

                const previewUrl = row.photo?.preview ?? row.photo?.src;
                if (previewUrl && previewUrl.startsWith('blob:')) {
                    URL.revokeObjectURL(previewUrl);
                }

                return { ...row, photo: entry };
            })
        );

        uploadVariationRowPhoto(targetRowKey, entry.id, file);
    };

    const clearVariationRowPhoto = (rowKey) => {
        setVariationRows((prev) =>
            prev.map((row) => {
                if (row.key !== rowKey) {
                    return row;
                }

                if (row.photo?.src && row.photo.src.startsWith('blob:')) {
                    URL.revokeObjectURL(row.photo.src);
                }

                return { ...row, photo: null };
            })
        );
    };

    const handleUpdateVariations = () => {
        setEditSnapshot(null);
        setMode('summary');
        setShowAddAttributeInput(false);
        setShowCustomOptionInput(false);
        setNewAttributeName('');
        setCustomOptionValue('');
    };

    const handleCancelEdit = () => {
        if (editSnapshot) {
            setAttributes(editSnapshot.attributes);
            setSelectedOptions(editSnapshot.selectedOptions);
            setExcludedKeys(editSnapshot.excludedKeys);
            setVariationRows(editSnapshot.variationRows);
        }
        setEditSnapshot(null);
        setMode('summary');
        setShowAddAttributeInput(false);
        setShowCustomOptionInput(false);
        setNewAttributeName('');
        setCustomOptionValue('');
    };

    const renderSummaryCard = (showEditButton) => (
        <CollapsibleCard
            title="Attributes and options you've selected"
            helper="Drag to rearrange"
            countBadge={allSelectedCount}
            defaultOpen
        >
            <div className="space-y-4">
                <div className="flex items-center justify-between">
                    <div className="text-sm text-gray-600">
                        <p className="font-medium text-gray-800">Number of variations</p>
                        <p>{allSelectedCount || 0}</p>
                    </div>
                    {showEditButton && (
                        <button
                            type="button"
                            onClick={handleEditAttributes}
                            className="rounded border border-gray-300 px-3 py-1 text-xs font-medium text-gray-700 hover:bg-gray-50"
                        >
                            Edit
                        </button>
                    )}
                </div>
                {attributes
                    .filter((attr) => (selectedOptions[attr.name] || []).length > 0)
                    .map((attr) => (
                        <div key={attr.id} className="space-y-1">
                            <p className="text-xs font-semibold uppercase text-gray-500">{attr.name}</p>
                            <div className="flex flex-wrap gap-1">
                                {(selectedOptions[attr.name] || []).map((option) => (
                                    <span
                                        key={`${attr.name}-${option}`}
                                        className="rounded border border-gray-200 bg-gray-50 px-2 py-1 text-xs text-gray-700"
                                    >
                                        {option}
                                    </span>
                                ))}
                        </div>
                    </div>
                ))}
                {attributes.every((attr) => (selectedOptions[attr.name] || []).length === 0) && (
                    <p className="text-sm text-gray-500">No options selected yet.</p>
                )}
            </div>
        </CollapsibleCard>
    );

    return (
        <div className="bg-white min-h-screen text-gray-800">
            <header className="bg-white border-b border-gray-200">
                <div className="flex items-center justify-between px-6 py-4">
                    <div className="flex flex-col gap-1">
                        <button
                            type="button"
                            onClick={handleBackNavigation}
                            className="self-start text-sm font-medium text-blue-600 hover:underline"
                        >
                            {fromListing ? 'Back to listing' : 'Back'}
                        </button>
                        <h1 className="text-2xl font-semibold text-gray-900">Variations</h1>
                        {categoryDetail?.name && (
                            <p className="text-xs text-gray-500">Category: {categoryDetail.name}</p>
                        )}
                    </div>
                    <a href="#comments" className="text-sm font-medium text-blue-600 hover:underline">
                        Send us your comments
                    </a>
                </div>
            </header>

            <main className="px-6 py-6 space-y-6">
                {mode === 'edit' ? (
                    <section>
                        <div className="mb-6">
                            <h2 className="text-xl font-semibold">Create your variations</h2>
                            <p className="mt-1 text-sm text-gray-600">
                                Select an attribute and then choose the options for the products you sell.
                            </p>
                        </div>

                        <div className="flex flex-col gap-6 lg:flex-row">
                            <div ref={attributePanelRef} className="flex-1 space-y-6">
                                <div className="rounded-md border border-gray-200 bg-white">
                                    <div className="border-b border-gray-200 px-6 py-4">
                                        <h3 className="text-base font-semibold text-gray-900">Variations</h3>
                                    </div>

                                    <div className="px-6 py-5">
                                        <div className="mb-6">
                                            <p className="mb-3 text-sm font-medium text-gray-600">Attributes</p>
                                            <div className="flex flex-wrap items-center gap-2">
                                                {attributes.map((attr) => (
                                                    <div
                                                        key={attr.id}
                                                        onClick={() => selectAttribute(attr.id)}
                                                        className={`relative flex cursor-pointer items-center gap-2 rounded-full px-3 py-1.5 text-sm transition ${
                                                            attr.selected
                                                                ? 'bg-gray-800 text-white'
                                                                : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
                                                        }`}
                                                    >
                                                        <span>{attr.name}</span>
                                                        <button
                                                            type="button"
                                                            onClick={(event) => {
                                                                event.stopPropagation();
                                                                removeAttribute(attr.name);
                                                            }}
                                                            className={`flex h-5 w-5 items-center justify-center rounded-full text-xs ${
                                                                attr.selected
                                                                    ? 'hover:bg-gray-700'
                                                                    : 'hover:bg-gray-300'
                                                            }`}
                                                        >
                                                            ✕
                                                        </button>
                                                        {attr.selected && (
                                                            <span className="absolute -bottom-2 left-1/2 h-0 w-0 -translate-x-1/2 border-l-[6px] border-r-[6px] border-t-[6px] border-transparent border-t-gray-800" />
                                                        )}
                                                    </div>
                                                ))}
                                                {!showAddAttributeInput && (
                                                    <button
                                                        type="button"
                                                        onClick={() => setShowAddAttributeInput(true)}
                                                        className="text-sm font-medium text-blue-600 hover:underline"
                                                    >
                                                        + Add
                                                    </button>
                                                )}
                                                {showAddAttributeInput && (
                                                    <div className="flex items-center gap-2">
                                                        <input
                                                            type="text"
                                                            value={newAttributeName}
                                                            onChange={(event) => setNewAttributeName(event.target.value)}
                                                            className="w-44 rounded border border-gray-300 px-3 py-1 text-sm focus:border-blue-500 focus:outline-none"
                                                            placeholder="Attribute name"
                                                        />
                                                        <button
                                                            type="button"
                                                            onClick={handleAddAttribute}
                                                            className="rounded border border-gray-300 px-2.5 py-1 text-xs font-medium text-gray-700 hover:bg-gray-50"
                                                        >
                                                            Add
                                                        </button>
                                                        <button
                                                            type="button"
                                                            onClick={() => {
                                                                setShowAddAttributeInput(false);
                                                                setNewAttributeName('');
                                                            }}
                                                            className="text-xs font-medium text-gray-500 hover:underline"
                                                        >
                                                            Cancel
                                                        </button>
                                                    </div>
                                                )}
                                            </div>
                                        </div>

                                        {currentAttribute ? (
                                            <div>
                                                <p className="mb-3 text-sm font-medium text-gray-600">
                                                    Options for {currentAttribute.name}
                                                </p>
                                                <div className="flex flex-wrap gap-2">
                                                    {(suggestionOptions.length ? suggestionOptions : currentOptions).map((option) => {
                                                        const selected = currentOptions.includes(option);
                                                        return (
                                                            <button
                                                                key={`${currentAttribute.name}-${option}`}
                                                                type="button"
                                                                onClick={() => toggleOption(currentAttribute.name, option)}
                                                                className={`rounded-full border px-4 py-1.5 text-sm transition ${
                                                                    selected
                                                                        ? 'border-gray-600 bg-gray-100 text-gray-800'
                                                                        : 'border-gray-300 bg-white text-blue-600 hover:border-gray-400'
                                                                }`}
                                                            >
                                                                {option}
                                                            </button>
                                                        );
                                                    })}
                                                </div>

                                                {!suggestionOptions.length && currentOptions.length === 0 && (
                                                    <p className="mt-4 text-xs text-gray-500">
                                                        There are no suggested options for this attribute yet. Create your own below.
                                                    </p>
                                                )}

                                                <div className="mt-4 space-y-2">
                                                    {!showCustomOptionInput && (
                                                        <button
                                                            type="button"
                                                            onClick={() => setShowCustomOptionInput(true)}
                                                            className="text-sm font-medium text-blue-600 hover:underline"
                                                        >
                                                            + Create your own
                                                        </button>
                                                    )}
                                                    {showCustomOptionInput && (
                                                        <div className="flex items-center gap-2">
                                                            <input
                                                                type="text"
                                                                value={customOptionValue}
                                                                onChange={(event) => setCustomOptionValue(event.target.value)}
                                                                className="w-52 rounded border border-gray-300 px-3 py-1 text-sm focus:border-blue-500 focus:outline-none"
                                                                placeholder="Enter a custom option"
                                                            />
                                                            <button
                                                                type="button"
                                                                onClick={handleAddCustomOption}
                                                                className="rounded border border-gray-300 px-2.5 py-1 text-xs font-medium text-gray-700 hover:bg-gray-50"
                                                            >
                                                                Add
                                                            </button>
                                                            <button
                                                                type="button"
                                                                onClick={() => {
                                                                    setShowCustomOptionInput(false);
                                                                    setCustomOptionValue('');
                                                                }}
                                                                className="text-xs font-medium text-gray-500 hover:underline"
                                                            >
                                                                Cancel
                                                            </button>
                                                        </div>
                                                    )}
                                                </div>
                                            </div>
                                        ) : (
                                            <p className="text-sm text-gray-500">Add an attribute to get started.</p>
                                        )}
                                    </div>
                                </div>

                                <div className="rounded-md border border-gray-200 bg-white p-5">
                                    <h4 className="mb-4 text-sm font-semibold text-gray-700">Selected options</h4>
                                    <div className="space-y-4">
                                        {attributes
                                            .filter((attr) => (selectedOptions[attr.name] || []).length > 0)
                                            .map((attr) => (
                                                <div key={attr.id}>
                                                    <div className="mb-2 flex items-center justify-between">
                                                        <span className="text-sm font-medium text-gray-800">{attr.name}</span>
                                                        <span className="text-xs text-gray-500">
                                                            {(selectedOptions[attr.name] || []).length} selected
                                                        </span>
                                                    </div>
                                                    <div className="flex flex-wrap gap-2">
                                                        {(selectedOptions[attr.name] || []).map((option) => (
                                                            <span
                                                                key={`${attr.name}-${option}`}
                                                                className="inline-flex items-center gap-2 rounded-full border border-gray-200 bg-gray-50 px-3 py-1 text-xs text-gray-700"
                                                            >
                                                                {option}
                                                                <button
                                                                    type="button"
                                                                    onClick={() => removeOption(attr.name, option)}
                                                                    className="text-gray-400 hover:text-gray-600"
                                                                >
                                                                    ✕
                                                                </button>
                                                            </span>
                                                        ))}
                                                    </div>
                                                </div>
                                            ))}

                                        {attributes.every((attr) => (selectedOptions[attr.name] || []).length === 0) && (
                                            <p className="text-sm text-gray-500">Select options to see them listed here.</p>
                                        )}
                                    </div>
                                </div>
                            </div>

                            <aside className="w-full lg:w-5/12">
                                <div className="space-y-4">{renderSummaryCard(false)}</div>
                            </aside>
                        </div>

                        <div className="mt-8 flex flex-col items-start gap-3 sm:flex-row">
                            <button
                                className="rounded bg-blue-600 px-6 py-2 text-sm font-medium text-white hover:bg-blue-700"
                                type="button"
                                onClick={handleUpdateVariations}
                            >
                                Update variations
                            </button>
                            <button
                                className="rounded border border-blue-600 px-6 py-2 text-sm font-medium text-blue-600 hover:bg-blue-50"
                                type="button"
                                onClick={handleCancelEdit}
                            >
                                Cancel
                            </button>
                        </div>
                    </section>
                ) : (
                    <>
                        <section>
                            <div className="mb-6">
                                <h2 className="text-xl font-semibold">Create your variations</h2>
                                <p className="mt-1 text-sm text-gray-600">
                                    Select an attribute and then choose the options for the products you sell.
                                </p>
                            </div>

                            <div className="space-y-6">{renderSummaryCard(true)}</div>
                        </section>

                        <section>
                            <CollapsibleCard
                                title="Photos"
                                helper="Add photos for each variation"
                                countBadge={defaultPhotos.length}
                                defaultOpen={false}
                            >
                                <div style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
                                    <div>
                                        <p style={{ fontWeight: 600, color: '#111820', marginBottom: '8px' }}>Add default photos</p>
                                        <span
                                            style={{
                                                display: 'inline-flex',
                                                alignItems: 'center',
                                                padding: '6px 16px',
                                                borderRadius: '20px',
                                                backgroundColor: '#f1f2f5',
                                                color: '#565e69',
                                                fontSize: '12px',
                                                fontWeight: 600
                                            }}
                                        >
                                            Default photos ({defaultPhotosCountLabel} photos)
                                        </span>
                                    </div>

                                    <input
                                        ref={defaultPhotosInputRef}
                                        type="file"
                                        multiple
                                        accept="image/*,video/*"
                                        style={{ display: 'none' }}
                                        onChange={handleDefaultPhotosInput}
                                    />

                                    <input
                                        ref={variationRowPhotoInputRef}
                                        type="file"
                                        accept="image/*"
                                        style={{ display: 'none' }}
                                        onChange={handleVariationRowPhotoInput}
                                    />

                                    <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
                                        <div style={{ marginBottom: '4px', fontSize: '14px', color: '#5f6675' }}>
                                            {defaultPhotos.length}/{MAX_DEFAULT_PHOTOS}
                                        </div>

                                        <div style={{ display: 'flex', gap: '16px' }}>
                                            <div
                                                style={{
                                                    border: defaultPhotos[0] ? 'none' : '3px dashed #aaa',
                                                    borderRadius: '12px',
                                                    width: '350px',
                                                    height: '350px',
                                                    display: 'flex',
                                                    flexDirection: 'column',
                                                    justifyContent: 'center',
                                                    alignItems: 'center',
                                                    textAlign: 'center',
                                                    backgroundColor: defaultPhotos[0] ? '#fff' : '#f9fafc'
                                                }}
                                            >
                                                {defaultPhotos[0] ? (
                                                    <EbayFilePreviewCard
                                                        a11yCancelUploadText="Cancel upload"
                                                        deleteText="Delete"
                                                        file={defaultPhotos[0]}
                                                        onDelete={() => removeDefaultPhoto(defaultPhotos[0].id)}
                                                    />
                                                ) : (
                                                    <div
                                                        role="button"
                                                        tabIndex={0}
                                                        // onClick={openDefaultPhotosPicker}
                                                        // onKeyDown={(event) => {
                                                        //     if (event.key === 'Enter' || event.key === ' ') {
                                                        //         event.preventDefault();
                                                        //         openDefaultPhotosPicker();
                                                        //     }
                                                        // }}
                                                        style={{
                                                            display: 'flex',
                                                            flexDirection: 'column',
                                                            alignItems: 'center',
                                                            justifyContent: 'center',
                                                            gap: '12px',
                                                            width: '100%',
                                                            height: '100%',
                                                            cursor: 'pointer'
                                                        }}
                                                    >
                                                        <AddImageIcon style={{ width: 24, height: 24 }} />
                                                        <p style={{ margin: 0, fontSize: '18px', fontWeight: 600 }}>Drag and drop files</p>
                                                        <div style={{ display: 'flex', gap: '12px' }}>
                                                            <EbayButton priority="tertiary" type="button" onClick={openDefaultPhotosPicker}>
                                                                Upload from computer
                                                            </EbayButton>
                                                        </div>
                                                    </div>
                                                )}
                                            </div>

                                            <div
                                                style={{
                                                    display: 'grid',
                                                    gridTemplateColumns: 'repeat(5, 115px)',
                                                    gridAutoRows: '115px',
                                                    gap: '8px',
                                                    alignContent: 'flex-start'
                                                }}
                                            >
                                                {defaultPhotos.slice(1).map((item) => (
                                                    <EbayFilePreviewCard
                                                        key={item.id}
                                                        a11yCancelUploadText="Cancel upload"
                                                        deleteText="Delete"
                                                        file={item}
                                                        onDelete={() => removeDefaultPhoto(item.id)}
                                                    />
                                                ))}

                                                {defaultPhotos.length < MAX_DEFAULT_PHOTOS && (
                                                    <div
                                                        onClick={openDefaultPhotosPicker}
                                                        role="button"
                                                        tabIndex={0}
                                                        onKeyDown={(event) => {
                                                            if (event.key === 'Enter' || event.key === ' ') {
                                                                event.preventDefault();
                                                                openDefaultPhotosPicker();
                                                            }
                                                        }}
                                                        style={{
                                                            background: '#f5f5f5',
                                                            borderRadius: '8px',
                                                            display: 'flex',
                                                            flexDirection: 'column',
                                                            justifyContent: 'center',
                                                            alignItems: 'center',
                                                            cursor: 'pointer',
                                                            fontSize: '14px',
                                                            color: '#5f6675'
                                                        }}
                                                    >
                                                        <AddImageIcon style={{ width: 24, height: 24 }} />
                                                        <span>Add</span>
                                                    </div>
                                                )}

                                                {Array.from({
                                                    length: Math.max(
                                                        0,
                                                        15 -
                                                            (defaultPhotos.slice(1).length +
                                                                (defaultPhotos.length < MAX_DEFAULT_PHOTOS ? 1 : 0))
                                                    )
                                                }).map((_, index) => (
                                                    <div
                                                        key={`default-photo-placeholder-${index}`}
                                                        style={{
                                                            background: '#f5f5f5',
                                                            borderRadius: '8px'
                                                        }}
                                                    />
                                                ))}
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </CollapsibleCard>
                        </section>

                        <section className="rounded-md border border-gray-200 bg-white">
                            <div className="flex flex-wrap items-center justify-between gap-3 border-b border-gray-200 px-6 py-4">
                                <h3 className="text-base font-semibold text-gray-900">
                                    Variation combinations ({variationRows.length})
                                </h3>
                                <div className="flex flex-wrap items-center gap-2">
                                    <BulkActionButton onClick={() => handleBulkUpdate('price', 'price')}>
                                        Enter price
                                    </BulkActionButton>
                                    <BulkActionButton onClick={() => handleBulkUpdate('quantity', 'quantity')}>
                                        Enter quantity
                                    </BulkActionButton>
                                    <BulkActionButton onClick={() => handleBulkUpdate('sku', 'SKU')}>
                                        Enter SKU
                                    </BulkActionButton>
                                    <BulkActionButton
                                        onClick={() =>
                                            handleDeleteRows(
                                                selectedRowKeys.length ? selectedRowKeys : variationRows.map((row) => row.key)
                                            )
                                        }
                                    >
                                        Delete
                                    </BulkActionButton>
                                    <button
                                        type="button"
                                        onClick={handleAddCombination}
                                        className="rounded border border-blue-600 bg-white px-3 py-2 text-xs font-medium text-blue-600 hover:bg-blue-50"
                                    >
                                        Add combination
                                    </button>
                                </div>
                            </div>

                            <div className="overflow-x-auto">
                                <table className="min-w-full divide-y divide-gray-200 text-sm">
                                    <thead className="bg-gray-50 text-xs font-semibold uppercase tracking-wide text-gray-500">
                                        <tr>
                                            <th className="w-12 px-4 py-3">
                                                <input
                                                    type="checkbox"
                                                    checked={areAllRowsSelected}
                                                    onChange={toggleSelectAllRows}
                                                    className="h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                                                />
                                            </th>
                                            <th className="px-4 py-3 text-left">Actions</th>
                                            <th className="px-4 py-3 text-left">Photos</th>
                                            <th className="px-4 py-3 text-left">SKU</th>
                                            <th className="px-4 py-3 text-left">UPC</th>
                                            {attributes.map((attr) => (
                                                <th key={`col-${attr.id}`} className="px-4 py-3 text-left">
                                                    {attr.name}
                                                </th>
                                            ))}
                                            <th className="px-4 py-3 text-left">Quantity</th>
                                            <th className="px-4 py-3 text-left">Price</th>
                                        </tr>
                                    </thead>
                                    <tbody className="divide-y divide-gray-200">
                                        {variationRows.map((row) => (
                                            <tr key={row.key} className="bg-white hover:bg-gray-50">
                                                <td className="px-4 py-3">
                                                    <input
                                                        type="checkbox"
                                                        checked={selectedRowKeys.includes(row.key)}
                                                        onChange={() => toggleRowSelection(row.key)}
                                                        className="h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-500"
                                                    />
                                                </td>
                                                <td className="px-4 py-3 text-sm">
                                                    <button
                                                        type="button"
                                                        onClick={() => handleDeleteRows([row.key])}
                                                        className="text-blue-600 hover:underline"
                                                    >
                                                        Delete
                                                    </button>
                                                </td>
                                                <td className="px-4 py-3">
                                                    {row.photo ? (
                                                        <div className="relative h-14 w-14 overflow-hidden rounded border border-gray-200">
                                                            <img
                                                                src={row.photo.src}
                                                                alt={row.photo.name}
                                                                className="h-full w-full object-cover"
                                                            />
                                                            <button
                                                                type="button"
                                                                onClick={() => clearVariationRowPhoto(row.key)}
                                                                className="absolute right-1 top-1 rounded bg-black/70 px-1 text-[10px] font-medium text-white"
                                                            >
                                                                Remove
                                                            </button>
                                                        </div>
                                                    ) : (
                                                        <button
                                                            type="button"
                                                            onClick={() => openVariationRowPhotoPicker(row.key)}
                                                            className="flex h-14 w-14 items-center justify-center rounded border border-dashed border-gray-300 bg-gray-50 text-[11px] font-medium text-gray-600 hover:border-gray-400"
                                                        >
                                                            Upload
                                                        </button>
                                                    )}
                                                </td>
                                                <td className="px-4 py-3">
                                                    <input
                                                        type="text"
                                                        value={row.sku}
                                                        onChange={(event) => updateRowField(row.key, 'sku', event.target.value)}
                                                        className="w-36 rounded border border-gray-300 px-3 py-1 text-sm focus:border-blue-500 focus:outline-none"
                                                    />
                                                </td>
                                                <td className="px-4 py-3">
                                                    <input
                                                        type="text"
                                                        value={row.upc}
                                                        onChange={(event) => updateRowField(row.key, 'upc', event.target.value)}
                                                        className="w-32 rounded border border-gray-300 px-3 py-1 text-sm focus:border-blue-500 focus:outline-none"
                                                    />
                                                </td>
                                                {attributes.map((attr) => (
                                                    <td key={`${row.key}-${attr.id}`} className="px-4 py-3 text-sm text-gray-700">
                                                        {row.attributeValues?.[attr.name] || '—'}
                                                    </td>
                                                ))}
                                                <td className="px-4 py-3">
                                                    <input
                                                        type="number"
                                                        min="0"
                                                        value={row.quantity}
                                                        onChange={(event) => updateRowField(row.key, 'quantity', event.target.value)}
                                                        className="w-24 rounded border border-gray-300 px-3 py-1 text-sm focus:border-blue-500 focus:outline-none"
                                                    />
                                                </td>
                                                <td className="px-4 py-3">
                                                    <div className="relative flex items-center">
                                                        <span className="absolute left-3 text-sm text-gray-500">$</span>
                                                        <input
                                                            type="number"
                                                            min="0"
                                                            step="0.01"
                                                            value={row.price}
                                                            onChange={(event) => updateRowField(row.key, 'price', event.target.value)}
                                                            className="w-28 rounded border border-gray-300 px-3 py-1 pl-6 text-sm focus:border-blue-500 focus:outline-none"
                                                        />
                                                    </div>
                                                </td>
                                            </tr>
                                        ))}

                                        {!variationRows.length && (
                                            <tr>
                                                <td colSpan={attributes.length + 7} className="px-4 py-10 text-center text-sm text-gray-500">
                                                    Select attribute options to generate variation combinations.
                                                </td>
                                            </tr>
                                        )}
                                    </tbody>
                                </table>
                            </div>
                        </section>

                        <section className="flex flex-col items-center justify-end gap-3 border-t border-gray-200 py-6 sm:flex-row">
                            <div className="flex gap-3">
                                <button
                                    className="rounded bg-blue-600 px-6 py-2 text-sm font-medium text-white hover:bg-blue-700"
                                    type="button"
                                    onClick={handleSaveAndClose}
                                >
                                    Save and close
                                </button>
                                <button
                                    className="rounded border border-blue-600 px-6 py-2 text-sm font-medium text-blue-600 hover:bg-blue-50"
                                    type="button"
                                    onClick={handleSaveAndPreview}
                                >
                                    Save and preview
                                </button>
                            </div>
                            <button
                                className="text-sm font-medium text-gray-600 hover:underline"
                                type="button"
                                onClick={handleBackNavigation}
                            >
                                Cancel
                            </button>
                        </section>
                    </>
                )}
            </main>
        </div>
    );
};

export default VariationForm;
