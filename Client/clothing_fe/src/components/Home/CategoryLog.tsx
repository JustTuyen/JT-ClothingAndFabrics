
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Typography from '@mui/material/Typography';
import CardActionArea from '@mui/material/CardActionArea';

//
//import Cat from '../../assets/placeCat.webp'
import { useEffect, useState } from 'react';
import api from '../../api/ApiHandler';
//

type Category = {
    id: number
    name: string
    imageURL: string
}

export default function CategoryLog(){
    const [categories, setCategories] = useState<Category[]>([]);
    useEffect(()=>{ 
        async function fetchCategories() {
        try{
            
            const {data} = await api.get(`/CategoryModels/card`);
            setCategories(data.results ?? data);
        }
        catch(error){
            console.log(error);
        }
    }

    fetchCategories()
    }, [])

    return(
        <>
        <div className="flex flex-col gap-8 items-center">
            <div className="flex justify-center">
                <p className="text-[18px] font-bold md:text-[24px]">DANH MỤC SẢN PHẨM</p>
            </div>
            <div className="p-4 grid 
            grid-cols-2
            md:grid-cols-4
            gap-4">
                {categories.map((cat, index) => 
                <Card key={cat.id ?? index} sx={{ maxWidth: 300, border: 'none', boxShadow: 'none'}}>
                    <CardActionArea>
                        <CardMedia
                        component="img"
                        height="140"
                        src={cat.imageURL}
                        alt={cat.name}
                        />
                        <CardContent sx={{textAlign: 'center'}}>
                            <Typography gutterBottom variant="h5" component="div">
                                {cat.name}
                            </Typography>
                        </CardContent>
                    </CardActionArea>
                </Card>
                )}
            </div>
        </div>
        </>
    )
}