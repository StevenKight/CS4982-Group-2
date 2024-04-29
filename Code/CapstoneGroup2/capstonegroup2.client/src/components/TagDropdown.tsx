import React, { useState } from 'react';

type Tag = {
  tagID: number;
  tagName: string;
};

interface Props {
  tags: Tag[];
  getSources(tags: Tag[] |
    null
  ): void;
}

const TagDropdown: React.FC<Props> = ({ tags, getSources }) => {
  const [selectedTags, setSelectedTags] = useState<number[]>([]); // Changed to number[] to store tagIds

  const handleSearch = () => {
    // Implement search logic based on selected tags
    console.log('Search based on selected tags:', selectedTags);
    const selectedTagObjects = selectedTags.map(tagId => tags.find(tag => tag.tagID === tagId)).filter(Boolean);
    getSources(selectedTagObjects as Tag[]);
    
  };

  const handleClearSelection = () => {
    getSources(null);
    setSelectedTags([]);
  };

  const toggleTagSelection = (tagId: number) => {
    if (selectedTags.includes(tagId)) {
      setSelectedTags(selectedTags.filter(id => id !== tagId));
    } else {
      setSelectedTags([...selectedTags, tagId]);
    }
  };

  return (
    <div style={{borderStyle:"solid", padding:"10px", marginRight:"30%", borderRadius:"4px",marginBottom:"1%"}}>
      <h2>Search by Tags:</h2>
    <div style={{ maxHeight:"120px",display:"grid", gridTemplateColumns: "auto auto"}}>
      <div 
      style={{ maxHeight: '50%', overflowY: 'auto', display:"grid", background:"grey", borderRadius:"4px"}}>
        {tags.map(tag => (
          <label key={tag.tagID}>
            <input
              type="checkbox"
              value={String(tag.tagID)}
              checked={selectedTags.includes(tag.tagID)}
              onChange={() => toggleTagSelection(tag.tagID)}
            />
            {tag.tagName}
          </label>
        ))}
      </div>
      <div style={{ width:"80%",marginLeft:"3%",display:"flex", justifyContent:"center"}}>
      <button disabled={selectedTags.length == 0} style={{ width:"50%",maxHeight:"40px",marginTop:"2%", marginBottom:"1%"}} onClick={handleSearch}>Search</button>
      <button disabled={selectedTags.length == 0} style={{width:"50%",maxHeight:"40px", marginLeft:"3%",marginTop:"2%"}} onClick={handleClearSelection}>Clear Selection</button>
        </div>
    </div>
    </div>
  );
};

export default TagDropdown;