
//
import * as React from 'react';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Modal from '@mui/material/Modal';
import Stack from '@mui/material/Stack';

//
import RemoveRedEyeOutlinedIcon from '@mui/icons-material/RemoveRedEyeOutlined';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
//
import NumberSpinner from './NumberSpinner';
import ItemGallery from './Item/ItemGallery';
import api from '../api/ApiHandler';
import { formatPrice } from '../store/Ult';
//
const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: '70%',
    bgcolor: 'white',
    border: '2px solid #00B7CD',
    boxShadow: 24,
    p: 4,
    outline: 'none',
    overflow: 'hidden',

    '@media (max-width: 810px)': {
        width: '90%',
        top: '50%',
        left: '50%',
    },
};

type VariantAttribute = {
    id: number
    attributeTypes: string      
    attributeValues: string     
}


type Variation = {
    id: number
    addPrice: number
    stockQuantity: number
    statusName: string | null
    variantAttributes: VariantAttribute[]  
}

type ImageGallery = {
    id: number
    displayOrder: number
    imageURL: string | null
}

type ProductDetail = {
    id: number
    name: string
    basePrice: number
    discountPercentage: number | null
    statusName: string | null
    variations: Variation[] 
    imageGalleries: ImageGallery[]     
}

type DetailCardProps = {
    id: number
}

export default function DetailCard({ id }: DetailCardProps){
    const [open, setOpen] = React.useState(false);
    const handleOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);
    const [selectedAttributes, setSelectedAttributes] = React.useState<Record<string, string>>({});

    const [product, setProduct] = React.useState<ProductDetail| null>(null);
    const [loading, setLoading] = React.useState(false);

    React.useEffect(()=>{
        if (!id) return;

        async function fetchProductDetail() {
            try{
                const {data} = await api.get(`/ProductModels/detail/${id}`);
                setProduct(data);
            } catch(error){
                console.log(error)
            } finally{
                setLoading(false)
            }
        }

       if (id) fetchProductDetail();
    }, [id])
    
   

    if (loading || !product) return(
        <>
        <p>Loading</p>
        </>
    );

    function handleSelect(attributeType: string, value: string) {
        setSelectedAttributes(prev => ({ ...prev, [attributeType]: value }));
    }

     function findMatchingVariation(
        variations: Variation[],
        selected: Record<string, string>
    ): Variation | undefined {
        return variations.find(v =>
            v.variantAttributes.every(attr => selected[attr.attributeTypes] === attr.attributeValues)
            && v.variantAttributes.length === Object.keys(selected).length
        );
    }

    function groupAttributesByType(variations: Variation[]): Record<string, string[]> {
        const grouped: Record<string, Set<string>> = {};
        for (const variation of variations) {
            for (const attr of variation.variantAttributes) {
                if (!grouped[attr.attributeTypes]) {
                    grouped[attr.attributeTypes] = new Set();
                }
                grouped[attr.attributeTypes].add(attr.attributeValues);
            }
        }

        const result: Record<string, string[]> = {};
        for (const key in grouped) {
            result[key] = Array.from(grouped[key]);
        }
        return result;
    }

    const matchedVariation = findMatchingVariation(product.variations, selectedAttributes);
    const groupedAttributes = groupAttributesByType(product.variations);

    return(
        <>
        <div className="">
            <Tooltip title="Xem trước">
                <IconButton sx={{ backgroundColor: '#BFC9D1' }} size="medium" onClick={handleOpen}>
                    <RemoveRedEyeOutlinedIcon fontSize="inherit" />
                </IconButton>
            </Tooltip>
            <Modal
            open={open}
            onClose={handleClose}
            aria-labelledby="modal-modal-title"
            aria-describedby="modal-modal-description"
            >
                {product?.statusName === 'Active' ? (
                <Box sx={style}>
                    <div className="grid grid-cols-1 md:grid-cols-2 max-h-[90vh] overflow-y-auto divide-y md:divide-y-0 md:divide-x divide-gray-100">
                        <div className="">
                            <ItemGallery images={product.imageGalleries}/>
                        </div>
                        <div className="shadow-md flex p-2 flex-col gap-2">
                            <div className="">
                                <div className="flex max-w-full">
                                    <p className="text-[18px] font-bold">
                                       {product?.name}
                                    </p>
                                </div>

                                {/* STATUS */}
                                <div className="flex gap-2">
                                    <p className="text-sm text-black">Tình trạng: <span className='font-bold'>Còn hàng</span></p>
                                    {/* <p>|</p>
                                    <p className="text-sm text-black">Thương hiệu: <span className='font-bold'>Old Sailor</span></p> */}
                                </div>
                            </div>
                            
                            {/* GIA SAN PHAM */}
                            <div className="text-[24px] font-bold p-4 rounded-2xl
                            text-[#DF301C] bg-[#EEEEEE]">
                                {formatPrice(product.basePrice)}
                            </div>

                            {/* KICH THUOC */}
                            {Object.entries(groupedAttributes).map(([attributeType, values]) => (
                            <div className="grid grid-cols-4 gap-2 p-4">
                                <div className='col-span-1 flex items-center'>
                                    <p className="font-black text-sm text-black">Kích thước:</p>
                                </div>

                                <div className="col-span-3 flex flex-wrap gap-2 justify-start">
                                    {values.map((value) => (
                                        <label key={value} className="cursor-pointer">
                                            <input
                                            type="radio"
                                            name={attributeType}
                                            value={value}
                                            checked={selectedAttributes[attributeType] === value}
                                            onChange={() => handleSelect(attributeType, value)}
                                            className="sr-only peer"
                                            />
                                            <span
                                            className="inline-block px-4 py-2 text-sm font-medium border rounded-lg 
                                            transition-all duration-200 border-gray-300 bg-white text-black
                                            hover:text-xl
                                            peer-checked:bg-[#DF301C] peer-checked:text-white peer-checked:border-[#DF301C]
                                            peer-focus-visible:ring-2 peer-focus-visible:ring-[#DF301C] peer-focus-visible:ring-offset-2"
                                            >
                                            {value}
                                            </span>
                                        </label>
                                    ))}
                                </div>
                            </div>
                            ))}



                            {/* SO LUONG */}
                            <div className="grid grid-cols-4 gap-2 p-4">
                                <div className='col-span-1 flex items-center'>
                                    <p className="font-black text-sm text-black">số lượng:</p>
                                </div>
                                <div className="">
                                    <Box
                                    sx={{
                                    display: 'flex',
                                    flexDirection: 'column',
                                    gap: 4,
                                    width: 150,
                                    justifyContent: 'start',
                                    }}
                                    >
                                        <NumberSpinner size="small" />
                                    </Box>
                                </div>
                            </div>

                            {/* ACTIONS */}
                            <div className="p-4">
                                <Stack spacing={2} direction="row">
                                    <Button size='large' fullWidth 
                                    variant="contained" 
                                    sx={{backgroundColor: '#FF9100', fontWeight: 'bold'}}
                                    >THÊM VÀO GIỎ</Button>
                                    <Button size='large' 
                                    fullWidth variant="contained"
                                    sx={{backgroundColor: '#DF301C',fontWeight: 'bold'}}
                                    >MUA NGAY</Button>
                                </Stack>
                            </div>
                        </div>
                    </div>
                </Box>
                ):(
                    <div className="">
                        <p>No data</p>
                    </div>
                )}
            </Modal>
        </div>
        </>

        
    )

    
}